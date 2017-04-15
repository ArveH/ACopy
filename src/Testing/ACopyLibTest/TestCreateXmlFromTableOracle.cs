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
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "BinaryDouble");
        }

        [TestMethod]
        public void TestBinaryFloat_When_Oracle()
        {
            var colDescr = "binary_float";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "BinaryFloat");
        }

        [TestMethod]
        public void TestBlob_When_Oracle()
        {
            var colDescr = "blob";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Blob");
        }

        [TestMethod]
        public void TestChar_When_Oracle()
        {
            var colDescr = "char(2)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Char", 2);
        }

        [TestMethod]
        public void TestClob_When_Oracle()
        {
            var colDescr = "clob";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "LongText");
        }

        [TestMethod]
        public void TestDate_When_Oracle()
        {
            var colDescr = "date";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "DateTime");
        }

        [TestMethod]
        public void TestLong_When_Oracle()
        {
            var colDescr = "long";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "OldText");
        }

        [TestMethod]
        public void TestNChar_When_Oracle()
        {
            var colDescr = "nchar(2)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "NChar", 2);
        }

        [TestMethod]
        public void TestNClob_When_Oracle()
        {
            var colDescr = "nclob";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "NLongText");
        }

        [TestMethod]
        public void TestNumber_When_Oracle()
        {
            var colDescr = "number";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Dec", 0, 0);
        }

        [TestMethod]
        public void TestNumber20_When_Oracle()
        {
            var colDescr = "number(20)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Dec", 20, 0);
        }

        [TestMethod]
        public void TestNumber19_4_When_Oracle()
        {
            var colDescr = "number(19,4)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Dec", 19, 4);
        }

        [TestMethod]
        public void TestNVarchar2_When_Oracle()
        {
            var colDescr = "nvarchar2(50)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "NVarchar", 50);
        }

        [TestMethod]
        public void TestRaw16_When_Oracle()
        {
            var colDescr = "raw(16)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Guid");
        }

        [TestMethod]
        public void TestRaw1000_When_Oracle()
        {
            var colDescr = "raw(1000)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Raw", 1000);
        }

        [TestMethod]
        public void TestTimestamp_When_Oracle()
        {
            var colDescr = "timestamp";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Timestamp", 6);
        }

        [TestMethod]
        public void TestTimestamp5_When_Oracle()
        {
            var colDescr = "timestamp(5)";
            var xmlDocument = CreateTableAndGetXml(colDescr);
            CheckColumnType(xmlDocument, "Timestamp", 5);
        }

        #region Private Helper functions

        private XmlDocument CreateTableAndGetXml(string colDescr)
        {
            var sqlTxt = $"create table {TableName} (col1 {colDescr})";
            Commands.ExecuteNonQuery(sqlTxt);
            var tableDefinition = DbSchema.GetTableDefinition(DbContext.ColumnTypeConverterForWrite,
                TableName);
            var xmlWriter = AXmlFactory.CreateWriter();
            xmlWriter.WriteSchema(tableDefinition, _schemaFilePath);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText(_schemaFilePath));
            return xmlDocument;
        }

        #endregion
    }
}