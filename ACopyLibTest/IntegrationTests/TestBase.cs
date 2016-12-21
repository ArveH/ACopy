using System.Collections.Generic;
using System.IO;
using ACopyLib.Reader;
using ACopyLib.Writer;
using ADatabase;
using TestSettingsHelper;

namespace ACopyLibTest.IntegrationTests
{
    public abstract class TestBase
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings(@"..\..\ConnectionStrings.json");
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;
        protected IColumnFactory ColumnFactory;
        private IAWriter _writer;
        private IAReader _reader;

        private const string Directory = @"e:\Temp\";
        protected string TestTable;
        private string _schemaFile;
        private string _dataFile;

        protected void Setup(string tableName)
        {
            TestTable = tableName;
            _schemaFile = tableName + ".aschema";
            _dataFile = tableName + ".adata";

            _writer = AWriterFactory.CreateInstance(DbContext);
            _reader = AReaderFactory.CreateInstance(DbContext);
            DbSchema = DbContext.PowerPlant.CreateDbSchema();
            Commands = DbContext.PowerPlant.CreateCommands();
            ColumnFactory = DbContext.PowerPlant.CreateColumnFactory();
            Cleanup();
        }

        public virtual void Cleanup()
        {
            DbSchema.DropTable(TestTable);
            DeleteFiles();
        }

        private void DeleteFiles()
        {
            if (File.Exists(Directory + _schemaFile))
            {
                File.Delete(Directory + _schemaFile);
            }
            if (File.Exists(Directory + _dataFile))
            {
                File.Delete(Directory + _dataFile);
            }
            if (System.IO.Directory.Exists(Directory + TestTable))
            {
                System.IO.Directory.Delete(Directory + TestTable, true);
            }
        }

        protected void CreateTestableWithAllTypes(string testTable)
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Bool, "bool_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Char, "char_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.DateTime, "date_col", false, "convert(datetime,'19000101',112)"),
                columnFactory.CreateInstance(ColumnType.Float, "float_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Guid, "guid_col", true, ""),
                columnFactory.CreateInstance(ColumnType.Int, "int_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int8, "int8_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int16, "int16_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int64, "int64_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.LongText, "longtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.Money, "money_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Raw, "raw_col", true, ""),
                columnFactory.CreateInstance(ColumnType.String, "string_col", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.Varchar, "varchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string stmt = string.Format("insert into {0} (bool_col, char_col, date_col, float_col, guid_col, int_col, int8_col, int16_col, int64_col, longtext_col, money_col, raw_col, string_col, varchar_col) ", testTable);
            if (DbContext.DbType == DbType.SqlServer)
            {
                stmt += "values (1,'NO', 'Feb 23 1900', 123.12345678, '3f2504e0-4f89-11d3-9a0c-0305e82c3301', 1234567890, 150, 12345, 123456789012345, N'Very long text with æøå', 123.123, convert(varbinary, 'Lots of bytes'), N'A unicode ﺽ string', 'A varchar string')";
            }
            else
            {
                stmt += "values (1,'NO', to_date('Feb 23 1900', 'Mon DD YYYY'), 123.12345678, hextoraw('3f2504e04f8911d39a0c0305e82c3301'), 1234567890, 150, 12345, 123456789012345, 'Very long text with æøå', 123.123, utl_raw.cast_to_raw('Lots of bytes'), 'A unicode ﺽ string', 'A varchar string')";
            }
            Commands.ExecuteNonQuery(stmt);
        }

        protected virtual void CreateTestTable1Row3Columns1Value(IColumn col, string testValue = "NULL")
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                col,
                columnFactory.CreateInstance(ColumnType.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
            };
            TableDefinition tableDefinition = new TableDefinition(TestTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string tmp = string.Format("insert into {0} (id, test_col, val) values (9, {1}, 'control value')", TestTable, testValue);
            Commands.ExecuteNonQuery(tmp);
        }

        protected void WriteAndRead()
        {
            _writer.Directory = Directory;
            _writer.Write(new List<string> { TestTable });

            DbSchema.DropTable(TestTable);

            _reader.Directory = Directory;
            int totalTables;
            int failedTables;
            _reader.Read(new List<string> { TestTable }, out totalTables, out failedTables);
        }

    }
}
