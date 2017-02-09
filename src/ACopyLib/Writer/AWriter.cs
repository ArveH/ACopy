using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ACopyLib.Resources;
using ACopyLib.U4Indexes;
using ACopyLib.Xml;
using ADatabase;
using ADatabase.Interfaces;
using ALogger;

namespace ACopyLib.Writer
{
    public class AWriter : IAWriter
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSchema _dbSchema;
        private readonly IALogger _logger;
        private const int RawBufferSize = 4000;

        public AWriter(IDbContext dbContext, IALogger logger)
        {
            _dbContext = dbContext;
            _dbSchema = _dbContext.PowerPlant.CreateDbSchema();
            _logger = logger;
        }

        private string _directory = @".\";
        public string Directory
        {
            get
            {
                return _directory;
            }
            set
            {
                _directory = value;
                if (_directory[_directory.Length-1] != '\\')
                {
                    _directory += "\\";
                }
            }
        }

        public string DataFileSuffix { get; set; } = Names.DefaultDataFileSuffix;
        public string SchemaFileSuffix { get; set; } = Names.DefaultSchemaFileSuffix;
        public bool UseU4Indexes { get; set; }
        public string ConversionsFile { get; set; }

        public int MaxDegreeOfParallelism { get; set; } = -1;

        public bool UseCompression { get; set; }

        public void Write(List<string> tableNameSearchList)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int tableCounter = 0;
            int errorCounter = 0;
            List<ITableShortInfo> tables = ExpandWildcards(tableNameSearchList);
            if (tables.Count > 1)
            {
                TableComparer comparer = new TableComparer();
                tables.Sort(comparer);
            }
            //tables
            //    .AsParallel()
            //    .AsOrdered()
            //    .ForAll(table =>
            //    {
            //        if (!WriteTable(table.Name))
            //        {
            //            errorCounter++;
            //        }
            //        tableCounter++;
            //    });
            Parallel.ForEach(
                tables, 
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, 
                table =>
                {
                    if (!WriteTable(table.Name))
                    {
                        errorCounter++;
                    }
                    tableCounter++;
                });
            //foreach (var table in ExpandWildcards(tableNameSearchList))
            //{
            //    WriteTable(table);
            //    tableCounter++;
            //}
            stopWatch.Stop();

            _logger.Write("");
            _logger.Write($"{tableCounter} tables copied. Total time: {stopWatch.Elapsed}");
            if (errorCounter > 0)
            {
                _logger.Write($"Found {errorCounter} tables with errors");
            }
        }

        private List<ITableShortInfo> ExpandWildcards(List<string> tableNameSearchList)
        {
            List<ITableShortInfo> tables = new List<ITableShortInfo>();
            try
            {
	            foreach (var tableSearchString in tableNameSearchList)
	            {
	                tables.AddRange(_dbSchema.GetTableNames(tableSearchString));
	            }
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
            }

            return tables;
        }

        public bool WriteTable(string tableName)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                _logger.Write($"{tableName,30} Started...");

                var columnsTypeConverter = _dbContext.PowerPlant.CreateColumnTypeConverter(ConversionsFile);

                var xmlSchema = XmlSchemaFactory.CreateInstance(_dbContext);
                if (UseU4Indexes) xmlSchema.U4Indexes = U4IndexesFactory.CreateInstance(_dbContext);
                ITableDefinition tableDefinition = xmlSchema.Write(Directory, columnsTypeConverter, tableName, SchemaFileSuffix);
	
	            if (tableDefinition.HasRawColumn)
	            {
	                System.IO.Directory.CreateDirectory($@"{Directory}{tableDefinition.Name}");
	            }

                long rowCounter = WriteTableDataToDataFile(tableDefinition, DataFileSuffix);

                stopWatch.Stop();

                string message = $"{tableDefinition.Name,30} {rowCounter,10} rows copied. Time: {stopWatch.Elapsed}";
                _logger.Write(message);
            }
            catch (Exception ex)
            {
                _logger.Write($"ERROR with table {tableName}");
                _logger.Write(ex);
                return false;
            }

            return true;
        }

        private long WriteTableDataToDataFile(ITableDefinition tableDefinition, string dataFileSuffix)
        {
            long rowCounter = 0;
            using (DataFileTextWriter dataWriter = new DataFileTextWriter(Directory + tableDefinition.Name + "." + dataFileSuffix, UseCompression))
            {
                string selectStmt = CreateSelectStatement(tableDefinition);
                IDataCursor cursor = _dbContext.PowerPlant.CreateDataCursor();
                try
                {
                    IDataReader reader = cursor.ExecuteReader(selectStmt, tableDefinition.HasRawColumn);
                    while (reader.Read())
                    {
                        WriteRow(rowCounter, reader, tableDefinition, dataWriter);
                        dataWriter.WriteLine();

                        if (rowCounter % 10000 == 0)
                        {
                            _logger.Write($"...... reached row {rowCounter} for {tableDefinition.Name} ......");
                        }

                        rowCounter++;
                    }
                }
                finally
                {
                    cursor.Close();
                }
            }
            return rowCounter;
        }

        private void WriteRow(long rowCounter, IDataReader reader, ITableDefinition tableDefinition, DataFileTextWriter dataWriter)
        {
            for (int i = 0; i < tableDefinition.Columns.Count; i++)
            {
                if (reader.IsDBNull(i))
                {
                    dataWriter.Write("NULL,");
                    continue;
                }

                if (tableDefinition.Columns[i].Type == ColumnTypeName.Blob)
                {
                    string rawFileName = $"i{rowCounter:D15}.raw";
                    dataWriter.Write(rawFileName);
                    WriteRawColumn(i, $@"{Directory}{tableDefinition.Name}\{rawFileName}", reader);
                }
                else
                {
                    dataWriter.Write(tableDefinition.Columns[i].ToString(reader.GetValue(i)));
                }
                dataWriter.Write(",");
            }
        }

        private void WriteRawColumn(int colId, string rawFileName, IDataReader reader)
        {
            byte[] buf = new byte[RawBufferSize];
            using (DataFileBinaryWriter writer = new DataFileBinaryWriter(rawFileName, UseCompression))
            {
                long startIndex = 0;
                long retval = reader.GetBytes(colId, startIndex, buf, 0, RawBufferSize);
                while (retval == RawBufferSize)
                {
                    writer.Write(buf, 0, (int)retval);
                    writer.Flush();
                    startIndex += retval;
                    retval = reader.GetBytes(colId, startIndex, buf, 0, RawBufferSize);
                }
                writer.Write(buf, 0, (int)retval);
                writer.Flush();
            }
        }

        private string CreateSelectStatement(ITableDefinition tableDefinition)
        {
            StringBuilder selectStmt = new StringBuilder("select ", 100 + tableDefinition.Columns.Count * 20);
            for (int i = 0; i < tableDefinition.Columns.Count; i++)
            {
                if (i > 0)
                {
                    selectStmt.Append(", ");
                }
                selectStmt.Append(tableDefinition.Columns[i].Name);
            }
            selectStmt.AppendFormat(" from {0}", tableDefinition.Name);

            return selectStmt.ToString();
        }
    }
}