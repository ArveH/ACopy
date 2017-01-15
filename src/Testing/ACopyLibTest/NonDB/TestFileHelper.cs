using System.Collections.Generic;
using ACopyLib.Utils;
using ACopyLib.Xml;
using ADatabase;
using ADatabase.Oracle;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestFileHelper
    {
        private string _folder;
        private const string SchemaFileSuffix = "aschema";

        [TestInitialize]
        public void Setup()
        {
            _folder = @".\";
        }

        private ITableDefinition CreateTableDefinition(string tableName)
        {
            IColumnFactory colFactory = new OracleColumnFactory();
            List<IColumn> columns = new List<IColumn>
            {
                colFactory.CreateInstance(ColumnTypeName.Int, "col1", false, "0")
            };

            return new TableDefinition(tableName, columns, "");
        }

        [TestMethod]
        public void TestGetSchemaFiles_When_TwoTables()
        {
            IAXmlWriter xmlWriter = AXmlFactory.CreateWriter();
            List<string> tableNames = new List<string> {"testtable1", "testtable2"};
            xmlWriter.WriteSchema(CreateTableDefinition(tableNames[0]), _folder + tableNames[0] + "." + SchemaFileSuffix);
            xmlWriter.WriteSchema(CreateTableDefinition(tableNames[1]), _folder + tableNames[1] + "." + SchemaFileSuffix);

            List<string> schemaFiles = FileHelper.GetSchemaFiles(_folder, tableNames, SchemaFileSuffix);
            schemaFiles.Should().Equal(_folder + tableNames[0] + ".aschema", _folder + tableNames[1] + "." + SchemaFileSuffix);
        }

        [TestMethod]
        public void TestGetSchemaFiles_When_TwoTables_And_WildcardMany()
        {
            IAXmlWriter xmlWriter = AXmlFactory.CreateWriter();
            xmlWriter.WriteSchema(CreateTableDefinition("testtable1"), _folder + "testtable1" + "." + SchemaFileSuffix);
            xmlWriter.WriteSchema(CreateTableDefinition("testtable2"), _folder + "testtable2" + "." + SchemaFileSuffix);

            List<string> tableNames = new List<string> { "testt%" };
            List<string> schemaFiles = FileHelper.GetSchemaFiles(_folder, tableNames, SchemaFileSuffix);
            schemaFiles.Should().Equal(_folder + "testtable1" + "." + SchemaFileSuffix, _folder + "testtable2" + "." + SchemaFileSuffix);
        }

        [TestMethod]
        public void TestGetSchemaFiles_When_TwoTables_And_WildcardOne()
        {
            IAXmlWriter xmlWriter = AXmlFactory.CreateWriter();
            xmlWriter.WriteSchema(CreateTableDefinition("testtable1"), _folder + "testtable1" + "." + SchemaFileSuffix);
            xmlWriter.WriteSchema(CreateTableDefinition("testtable2"), _folder + "testtable2" + "." + SchemaFileSuffix);

            List<string> tableNames = new List<string> { "testtable_" };
            List<string> schemaFiles = FileHelper.GetSchemaFiles(_folder, tableNames, SchemaFileSuffix);
            schemaFiles.Should().Equal(_folder + "testtable1" + "." + SchemaFileSuffix, _folder + "testtable2" + "." + SchemaFileSuffix);
        }
    }
}
