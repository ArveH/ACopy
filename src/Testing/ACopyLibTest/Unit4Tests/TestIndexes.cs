using ACopyLib.U4Indexes;
using ACopyLib.Xml;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;

namespace ACopyLibTest.Unit4Tests
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

        private void InsertIntoIndexesTable(string indexesTable, string indexName, string columnList)
        {
            string insertStmt =
                $"insert into {indexesTable} (index_name, table_name, column_list, location_name, unique_flag, db_name) values ('{indexName}', '{TestTable}', '{columnList}', 'default', 0, 'ORACLE')";
            Commands.ExecuteNonQuery(insertStmt);
        }

        // TestMethod
        protected void TestIndex()
        {
            TestTableCreator.CreateTestTableWithIndex(DbContext, TestTable);

            WriteAndRead();

            DbSchema.IsIndex("i_" + TestTable, TestTable).Should().BeTrue("because index should be recreated");
        }

        private IXmlSchema SetupXmlSchemaAndTable(bool setU4Indexes = true)
        {
            var xmlSchema = XmlSchemaFactory.CreateInstance(DbContext);
            if (setU4Indexes)
            {
                var u4Indexes = U4IndexesFactory.CreateInstance(DbContext);
                u4Indexes.AagTableName = Aagindex;
                u4Indexes.AsysTableName = Asysindex;
                xmlSchema.U4Indexes = u4Indexes;
            }
            xmlSchema.XmlWriter = AXmlFactory.CreateWriter();
            TestTableCreator.CreateTestTableWithIndex(DbContext, TestTable);
            TestTableCreator.CreateIndexTables(DbContext, Asysindex, Aagindex);
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
        protected void TestIndexes_When_IndexExistsInAsysIndex_But_U4IndexesIsNotSet()
        {
            var xmlSchema = SetupXmlSchemaAndTable(false);

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "id, val");

            var tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(1, "because the extra index from asysindex should be ignored");

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
