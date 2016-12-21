using System.Collections.Generic;
using ACopyLib.U4Indexes;
using ACopyLib.Xml;
using ADatabase;
using FluentAssertions;

namespace ACopyLibTest.IntegrationTests
{
    public class TestIndexes: TestBase
    {
        private const string Asysindex = "testasysindex";
        private const string Aagindex = "testaagindex";

        public virtual void Setup()
        {
            base.Setup("testindexes");
        }

        public override void Cleanup()
        {
            DbSchema.DropTable(Asysindex);
            DbSchema.DropTable(Aagindex);
            base.Cleanup();
        }

        private void CreateIndexTables()
        {
            DbSchema.DropTable(Asysindex);
            DbSchema.DropTable(Aagindex);
            var columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.String, "index_name", 60, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.String, "table_name", 60, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.String, "column_list", 510, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.String, "location_name", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.Int8, "unique_flag", false, "0"),
                columnFactory.CreateInstance(ColumnType.String, "db_name", 20, false, "' '", "Danish_Norwegian_CI_AS")
            };
            DbSchema.CreateTable(new TableDefinition(Aagindex, columns, ""));
            DbSchema.CreateTable(new TableDefinition(Asysindex, columns, ""));
        }

        private void InsertIntoIndexesTable(string indexesTable, string indexName, string columnList)
        {
            string insertStmt =
                $"insert into {indexesTable} (index_name, table_name, column_list, location_name, unique_flag, db_name) values ('{indexName}', '{TestTable}', '{columnList}', 'default', 0, 'ORACLE')";
            Commands.ExecuteNonQuery(insertStmt);
        }

        private void CreateTestTableWithIndex()
        {
            var columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int64, "id2", false, "0"),
                columnFactory.CreateInstance(ColumnType.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
            };
            var tableDefinition = new TableDefinition(TestTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string tmp = $"insert into {TestTable} (id, val) values (9, 'control value')";
            Commands.ExecuteNonQuery(tmp);
            Commands.ExecuteNonQuery($"create unique index {"i_" + TestTable} on {TestTable}(id)");
        }

        // TestMethod
        protected void TestIndex()
        {
            CreateTestTableWithIndex();

            WriteAndRead();

            DbSchema.IsIndex("i_" + TestTable, TestTable).Should().BeTrue("because index should be recreated");
        }

        private IXmlSchema SetupXmlSchemaAndTable()
        {
            var xmlSchema = XmlSchemaFactory.CreateInstance(DbContext);
            var u4Indexes = U4IndexesFactory.CreateInstance(DbContext);
            u4Indexes.AagTableName = Aagindex;
            u4Indexes.AsysTableName = Asysindex;
            xmlSchema.U4Indexes = u4Indexes;
            xmlSchema.XmlWriter = AXmlFactory.CreateWriter();
            CreateTestTableWithIndex();
            CreateIndexTables();
            return xmlSchema;
        }

        private void RecreateTableAndIndexes(ITableDefinition tableDefinition)
        {
            DbSchema.DropTable(TestTable);
            DbSchema.CreateTable(tableDefinition);
            DbSchema.CreateIndexes(tableDefinition.Indexes);
        }

        private ITableDefinition WriteAndReadSchema(IXmlSchema xmlSchema)
        {
            xmlSchema.Write(".\\", TestTable, "aschema");
            var tableDefinition = xmlSchema.GetTableDefinition(".\\" + TestTable + ".aschema");
            return tableDefinition;
        }

        // TestMethod
        protected void TestIndexes_When_IndexExistsInAsysIndex()
        {
            var xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "id, val");

            var tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(2, "because an extra index was added from asysindex");

            RecreateTableAndIndexes(tableDefinition);
        }

        // TestMethod
        protected void TestIndexes_When_IndexExistsInBothAagAndAsysIndex_Then_OnlyAAgAdded()
        {
            var xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "id, val");
            InsertIntoIndexesTable(Aagindex, "i_" + TestTable + "1", "id2, val");

            var tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(2, "because index in aag should override asys (when indexes has same name)");
            tableDefinition.Indexes[1].Columns[0].Name.Should().Be("id2", "because index with id from asys should be overridden");

            RecreateTableAndIndexes(tableDefinition);
        }

        // TestMethod
        protected void TestFunctionBasedIndex()
        {
            var xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "to_number(to_char(id)||''00'')");

            var tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(2, "because index we have an index in asysindex in addition");
            tableDefinition.Indexes[1].Columns[0].Name.Should().Be("expression", "because index is function based");

            RecreateTableAndIndexes(tableDefinition);
        }

        // TestMethod
        protected void TestIndexes_When_SameIndexInAagIndexAndOnTable_Then_OnTableWins()
        {
            var xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable, "id2");

            var tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(1, "because index on table should override aagindex");
            tableDefinition.Indexes[0].Columns[0].Name.Should().Be("id", "because index on table should win");

            RecreateTableAndIndexes(tableDefinition);
        }
    }
}
