using System.Collections.Generic;
using System.IO;
using System.Xml;
using ACopyLib.Reader;
using ACopyLib.Writer;
using ACopyTestHelper;
using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace ACopyLibTest
{
    [TestClass]
    public class TestWriteReadOracle: TestCopyLibBase
    {
        private OraTableCreator _oraTableCreator;
        private IAWriter _writer;
        private IAReader _reader;
        private string _schemaFileName;
        private string _dataFileName;
        private string _blobFileName;

        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            _oraTableCreator = new OraTableCreator(DbContext);
            TableName = _oraTableCreator.TableName;

            _writer = AWriterFactory.CreateInstance(DbContext);
            _writer.Directory = ".\\";
            _reader = AReaderFactory.CreateInstance(DbContext);
            _reader.Directory = ".\\";

            _schemaFileName = $@".\{TableName}.{_writer.SchemaFileSuffix}";
            _dataFileName = $@".\{TableName}.{_writer.DataFileSuffix}";
            _blobFileName = $@".\{TableName}\i000000000000000.raw";

            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
            File.Delete(_schemaFileName);
            File.Delete(_dataFileName);
        }

        [TestMethod]
        public void TestBinaryDouble_When_Oracle()
        {
            _oraTableCreator.BinaryDoubleColumn();

            WriteAndVerify(
                "BinaryDouble",
                TestTableCreator.GetBinaryFloatSqlValue());

            ReadAndVerify("binary_double", null, null, null);
        }


        #region Private

        private void CheckDataFile(string expectedData)
        {
            var fileContent = File.ReadAllText(_dataFileName);
            fileContent.Should().Contain(expectedData);
        }

        private XmlDocument GetXmlDocument()
        {
            _writer.WriteTable(TableName);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText(_schemaFileName));
            return xmlDocument;
        }

        private void WriteAndVerify(
            string expectedType,
            string expectedData)
        {
            var xmlDocument = GetXmlDocument();
            CheckColumnType(xmlDocument, expectedType);
            CheckDataFile(expectedData);
        }

        private void WriteAndVerify(
            string expectedType,
            string tagName,
            int size,
            string expectedData)
        {
            var xmlDocument = GetXmlDocument();
            CheckColumnType(xmlDocument, expectedType, tagName, size);
            CheckDataFile(expectedData);
        }

        private void WriteAndVerify(
            string exptectedType,
            int prec,
            int scale,
            string expectedData)
        {
            var xmlDocument = GetXmlDocument();
            CheckColumnType(xmlDocument, exptectedType, prec, scale);
            CheckDataFile(expectedData);
        }

        private void ReadAndVerify(
            string expectedType,
            int? expectedLength,
            int? expectedPrec,
            int? expectedScale)
        {
            _reader.Read(new List<string>() { TableName }, out int tableCounter, out int errorCounter);
            DbSchema.GetRawColumnDefinition(TableName, "col1", out string type, out int length, out int prec, out int scale);

            type.ToLower().Should().Be(expectedType);
            if (expectedLength.HasValue) length.Should().Be(expectedLength.Value);
            if (expectedPrec.HasValue) prec.Should().Be(expectedPrec.Value);
            if (expectedScale.HasValue) scale.Should().Be(expectedScale.Value);
        }

        #endregion
    }
}