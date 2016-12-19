using System;
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
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
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
            string insertStmt = String.Format("insert into {0} (index_name, table_name, column_list, location_name, unique_flag, db_name) values ('{1}', '{2}', '{3}', 'default', 0, 'ORACLE')", 
                indexesTable, indexName, TestTable, columnList);
            Commands.ExecuteNonQuery(insertStmt);
        }

        private void CreateTestTableWithIndex()
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int64, "id2", false, "0"),
                columnFactory.CreateInstance(ColumnType.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
            };
            TableDefinition tableDefinition = new TableDefinition(TestTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string tmp = string.Format("insert into {0} (id, val) values (9, 'control value')", TestTable);
            Commands.ExecuteNonQuery(tmp);
            Commands.ExecuteNonQuery(string.Format("create unique index {0} on {1}(id)", "i_" + TestTable, TestTable));
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
            IXmlSchema xmlSchema = XmlSchemaFactory.CreateInstance(DbContext);
            IU4Indexes u4Indexes = U4IndexesFactory.CreateInstance(DbContext);
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
            ITableDefinition tableDefinition = xmlSchema.GetTableDefinition(".\\" + TestTable + ".aschema");
            return tableDefinition;
        }

        // TestMethod
        protected void TestIndexes_When_IndexExistsInAsysIndex()
        {
            IXmlSchema xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "id, val");

            ITableDefinition tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(2, "because an extra index was added from asysindex");

            RecreateTableAndIndexes(tableDefinition);
        }

        // TestMethod
        protected void TestIndexes_When_IndexExistsInBothAagAndAsysIndex_Then_OnlyAAgAdded()
        {
            IXmlSchema xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "id, val");
            InsertIntoIndexesTable(Aagindex, "i_" + TestTable + "1", "id2, val");

            ITableDefinition tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(2, "because index in aag should override asys (when indexes has same name)");
            tableDefinition.Indexes[1].Columns[0].Name.Should().Be("id2", "because index with id from asys should be overridden");

            RecreateTableAndIndexes(tableDefinition);
        }

        // TestMethod
        protected void TestFunctionBasedIndex()
        {
            IXmlSchema xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable + "1", "to_number(to_char(id)||''00'')");

            ITableDefinition tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(2, "because index we have an index in asysindex in addition");
            tableDefinition.Indexes[1].Columns[0].Name.Should().Be("expression", "because index is function based");

            RecreateTableAndIndexes(tableDefinition);
        }

        // TestMethod
        protected void TestIndexes_When_SameIndexInAagIndexAndOnTable_Then_OnTableWins()
        {
            IXmlSchema xmlSchema = SetupXmlSchemaAndTable();

            InsertIntoIndexesTable(Asysindex, "i_" + TestTable, "id2");

            ITableDefinition tableDefinition = WriteAndReadSchema(xmlSchema);

            tableDefinition.Indexes.Count.Should().Be(1, "because index on table should override aagindex");
            tableDefinition.Indexes[0].Columns[0].Name.Should().Be("id", "because index on table should win");

            RecreateTableAndIndexes(tableDefinition);
        }
    }
}
