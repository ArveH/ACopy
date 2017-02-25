using System.IO;
using System.Xml;
using ACopyLib.Xml;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestCreateXmlFromTableOracle : TestCopyLibBase
    {
        private string _schemaFilePath;

        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            TableName = "htablewithallcolumns";
            _schemaFilePath = $"./{TableName}.aschema";
            base.Setup();
            DbSchema.DropTable(TableName);
            File.Delete(_schemaFilePath);
            Commands = PowerPlant.CreateCommands();
            //CreateTableWithAllColumns(false);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        public void TestBinaryDouble_When_Oracle()
        {
            var colDescr = "binary_double";
            var sqlTxt = $"create table {TableName} (col1 {colDescr})";
            Commands.ExecuteNonQuery(sqlTxt);
            var tableDefinition = DbSchema.GetTableDefinition(DbContext.ColumnTypeConverterForWrite,
                TableName);
            var xmlWriter = AXmlFactory.CreateWriter();
            xmlWriter.WriteSchema(tableDefinition, _schemaFilePath);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText(_schemaFilePath));
            var typeNode = xmlDocument.DocumentElement?.SelectSingleNode("/Table/Columns/Column/Type");
            typeNode.Should().NotBeNull("because column has to have Type");
            typeNode?.InnerText.Should().Be("BinaryDouble");
        }
    }
}