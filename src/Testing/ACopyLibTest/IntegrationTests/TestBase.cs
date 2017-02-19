using System.Collections.Generic;
using System.IO;
using ACopyLib.Reader;
using ACopyLib.Writer;
using ACopyTestHelper;
using ADatabase;

namespace ACopyLibTest.IntegrationTests
{
    public abstract class TestBase
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;
        protected IColumnFactory ColumnFactory;
        private IAWriter _writer;
        private IAReader _reader;

        private const string Directory = @".\";
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

        protected virtual void CreateTestTable1Row3Columns1Value(IColumn col, string testValue = "NULL")
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", 0, 20, 0, false, "0", ""),
                col,
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
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
