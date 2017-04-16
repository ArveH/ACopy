using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACopyLib.DataReader;
using ACopyLib.Resources;
using ACopyLib.Utils;
using ACopyLib.Writer;
using ACopyLib.Xml;
using ADatabase;
using ADatabase.Exceptions;
using ADatabase.Interfaces;
using ALogger;

namespace ACopyLib.Reader
{
    public class AReader : IAReader
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSchema _dbSchema;
        private readonly IALogger _logger;


        private string _directory = ".\\";
        public string Directory
        {
            get { return _directory; }
            set 
            { 
                _directory = value;
                if (_directory[_directory.Length - 1] != '\\')
                {
                    _directory += "\\";
                }
            }
        }

        public bool CreateClusteredIndex { get; set; }
        public int MaxDegreeOfParallelism { get; set; } = -1;
        public int BatchSize { get; set; }
        public string Collation { get; set; }
        public string DataFileSuffix { get; set; } = Names.DefaultDataFileSuffix;
        public string SchemaFileSuffix { get; set; } = Names.DefaultSchemaFileSuffix;


        public AReader(IDbContext dbContext, IALogger logger)
        {
            _dbContext = dbContext;
            _dbSchema = _dbContext.PowerPlant.CreateDbSchema();
            _logger = logger;
        }

        public void Read(List<string> tableNameSearchList, out int totalTables, out int failedTables)
        {
            var tableCounter = 0;
            var errorCounter = 0;
            Parallel.ForEach(
                FileHelper.GetSchemaFiles(Directory, tableNameSearchList, SchemaFileSuffix),
                new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                schemaFile =>
                {
                    if (!ReadTable(schemaFile))
                    {
                        errorCounter++;
                    }
                    tableCounter++;
                });
            totalTables = tableCounter;
            failedTables = errorCounter;
            //foreach (var schemaFile in FileHelper.GetSchemaFiles(Directory, tableNameSearchList))
            //{
            //    ReadTable(schemaFile);
            //    tableCounter++;
            //}
        }

        private bool ReadTable(string schemaFile)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            string currentTableName;
            long rowCounter;
            try
            {
                var tableDefinition = GetTableDefinition(schemaFile);
                var raw16Columns = _dbContext.DbType == DbTypeName.Oracle ? tableDefinition.GetRaw16Columns() : new List<string>();
                tableDefinition.Columns
                    .FindAll(c => raw16Columns.Contains(c.Name))
                    .ForEach(c => c.Details["Length"] = 17);

                _dbSchema.DropTable(tableDefinition.Name);
                _dbSchema.CreateTable(tableDefinition);

                ClusteredIndexOnAgrtid(tableDefinition);

                currentTableName = tableDefinition.Name;
                _logger.Write($"{currentTableName,30} Started...");
                var fileName = GetDataFileName(currentTableName);
                rowCounter = BulkLoadData(tableDefinition, fileName);
                WhenOracleAlterRaw16Columns(tableDefinition.Name, raw16Columns, 16);

                _dbSchema.CreateIndexes(tableDefinition.Indexes);
            }
            catch (Exception ex)
            {
                _logger.Write($"ERROR while reading table. Current schema file: {schemaFile}:");
                _logger.Write(ex);
                return false;
            }

            stopWatch.Stop();
            var message = $"{currentTableName,30} {rowCounter,10} rows read. Time: {stopWatch.Elapsed}";
            _logger.Write(message);

            return true;
        }

        private void ClusteredIndexOnAgrtid(ITableDefinition tableDefinition)
        {
            if (CreateClusteredIndex && _dbContext.DbType == DbTypeName.SqlServer)
            {
                var index = _dbContext.PowerPlant.CreateIndexDefinition(
                    "aic_" + tableDefinition.Name,
                    tableDefinition.Name,
                    tableDefinition.Location,
                    true, /* is unique */
                    0, /* id */
                    true /* is clustered */);
                index.Columns = new List<IIndexColumn> {IndexColumnFactory.CreateInstance("agrtid")};
                _dbSchema.CreateIndex(index);
            }
        }

        private ITableDefinition GetTableDefinition(string schemaFile)
        {
            var tableDefinition = XmlSchemaFactory.CreateInstance(_dbContext).GetTableDefinition(schemaFile);
            SetCollationIfUseCollationParameterUsed(tableDefinition);
            return tableDefinition;
        }

        private void SetCollationIfUseCollationParameterUsed(ITableDefinition tableDefinition)
        {
            if (_dbContext.DbType != DbTypeName.SqlServer)
            {
                return;
            }

            if (Collation == null)
            {
                tableDefinition.SetCollation(_dbSchema.GetCollation());
            }
            else if (Collation != "")
            {
                tableDefinition.SetCollation(Collation);
            }
        }

        private long BulkLoadData(ITableDefinition tableDefinition, string fileName)
        {
            for (var i = 0; i < 30; i++)
            {
                var bulkCopy = _dbContext.PowerPlant.CreateFastCopy();
                if (BatchSize > 0)
                {
                    bulkCopy.BatchSize = BatchSize;
                }
                try
                {
	                using (var reader = ADataReaderFactory.CreateInstance(fileName, tableDefinition, bulkCopy.LargeBlobSize))
	                {
	                    return bulkCopy.LoadData(reader, fileName, tableDefinition);
	                }
                }
                catch (Exception ex)
                {
                    if (_dbContext.DbType == DbTypeName.SqlServer && ADatabaseException.ShouldThrottle(ex))
                    {
                        _logger.Write($"Throttling down when load table {tableDefinition.Name}, round {i}");
                        _logger.Write("Error causing throttle:");
                        _logger.Write(ex);
                        Thread.Sleep(10000 + i * i * i * 1000);
                    }
                    else
                    {
                        _logger.Write($"ERROR when load table {tableDefinition.Name}, round {i}");
                        throw;
                    }
                }
            }

            throw new ADatabaseException("Failed throttling down when loading " + tableDefinition.Name);
        }

        private string GetDataFileName(string tableName)
        {
            var fileName = Directory + tableName + "." + DataFileSuffix;
            var compressedFileName = fileName + DataFileWriter.CompressionFileEnding;
            if (File.Exists(compressedFileName))
            {
                return compressedFileName;
            }
            return fileName;
        }

        // Workaround to get OracleDataReader to work with raw(16):
        //    create column as raw(17)
        //    copy data
        //    resize to raw(16)
        // This is because of a bug with the oracle reader.
        private void WhenOracleAlterRaw16Columns(string tableName, List<string> colNames, int length)
        {
            if (_dbContext.DbType != DbTypeName.Oracle || colNames.Count == 0) return;

            var commands = _dbContext.PowerPlant.CreateCommands();
            colNames.ForEach(colName => commands.ExecuteNonQuery($"alter table {tableName} modify {colName} raw(16)"));
        }
    }
}