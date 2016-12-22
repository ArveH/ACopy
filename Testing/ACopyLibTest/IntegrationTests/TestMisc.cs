using System.Collections.Generic;
using System.IO;
using ACopyLib.Reader;
using ACopyLib.Writer;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;

namespace ACopyLibTest.IntegrationTests
{
    public abstract class TestMisc
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings(@"..\..\ConnectionStrings.json");
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;

        private const string Directory = @"e:\Temp\";
        protected const string TestTable = "testmisc";
        private const string SchemaFile = "testmisc.aschema";
        private const string DataFile = "testmisc.adata";

        public abstract void Setup();

        public virtual void Cleanup()
        {
            DbSchema.DropTable(TestTable);
            DeleteFiles();
        }

        protected void DeleteFiles()
        {
            if (File.Exists(Directory + SchemaFile))
            {
                File.Delete(Directory + SchemaFile);
            }
            if (File.Exists(Directory + DataFile))
            {
                File.Delete(Directory + DataFile);
            }
            if (System.IO.Directory.Exists(Directory + TestTable))
            {
                System.IO.Directory.Delete(Directory + TestTable, true);
            }
        }

        private void CreateTestTableForDifferentStrings(string stringValue)
        {
            CreateTable();
            Commands.ExecuteNonQuery(string.Format("insert into {0} (id, seq_no, val) values (0, 1, '{1}')", TestTable, stringValue));
        }

        private void CreateTable()
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int, "seq_no", false, "0"),
                columnFactory.CreateInstance(ColumnType.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
            };
            TableDefinition tableDefinition = new TableDefinition(TestTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
        }

        private void WriteCompressedEmptyTable()
        {
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.UseCompression = true;
            writer.Write(new List<string> { TestTable });
        }

        private void Read()
        {
            IAReader reader = AReaderFactory.CreateInstance(DbContext);
            reader.Directory = Directory;
            int totalTables;
            int failedTables;
            reader.Read(new List<string> { TestTable }, out totalTables, out failedTables);
        }

        //TestMethod
        protected void TestWriteRead_When_UseCompression_And_EmptyTable()
        {
            CreateTable();
            WriteCompressedEmptyTable();
            DbSchema.DropTable(TestTable);
            Read();

            DbSchema.IsTable(TestTable).Should().BeTrue("because table was recreated");
        }

        //TestMethod
        protected void TestWriteRead_When_UseCompression_And_StringWithQuote()
        {
            CreateTestTableForDifferentStrings("O''line");
            WriteCompressedEmptyTable();
            DbSchema.DropTable(TestTable);
            Read();

            DbSchema.IsTable(TestTable).Should().BeTrue("because table was recreated");
        }
    }
}
