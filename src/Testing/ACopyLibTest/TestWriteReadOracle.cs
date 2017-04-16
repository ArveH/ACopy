using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void TestBinaryDouble()
        {
            _oraTableCreator.BinaryDoubleColumn();

            WriteAndVerify(
                "BinaryDouble",
                // Check the first 11 decimals to avoid double rounding problems
                TestTableCreator.BinaryDoubleValue.ToString("F11", CultureInfo.InvariantCulture));

            ReadAndVerify("binary_double", null, null, null);
        }

        [TestMethod]
        public void TestBinaryFloat()
        {
            _oraTableCreator.BinaryFloatColumn();

            WriteAndVerify(
                "BinaryFloat",
                // Test value for BinaryFloat has 5 decimals, so check the first 4 to handle float rounding problems
                TestTableCreator.BinaryFloatValue.ToString("F4", CultureInfo.InvariantCulture));

            ReadAndVerify("binary_float", null, null, null);
        }

        [TestMethod]
        public void TestBlob()
        {
            _oraTableCreator.Blob();

            WriteAndVerify(
                "Blob",
                "i000000000000000.raw");
            var blobContent = File.ReadAllText(_blobFileName);
            blobContent.Should().Be(TestTableCreator.BlobValue);
            CheckThatDetailDoesNotExist(_schemaFileName, "Length");

            ReadAndVerify("blob", null, null, null);
        }

        [TestMethod]
        public void TestChar10()
        {
            _oraTableCreator.CharColumn(10);

            WriteAndVerify(
                "Char",
                "Length", 10,
                TestTableCreator.GetCharSqlValue());

            ReadAndVerify("char", 10, null, null);
        }

        [TestMethod]
        public void TestClob()
        {
            _oraTableCreator.Clob();

            WriteAndVerify(
                "LongText",
                TestTableCreator.GetLongTextSqlValue());
            CheckThatDetailDoesNotExist(_schemaFileName, "Length");

            ReadAndVerify("clob", null, null, null);
        }

        [TestMethod]
        public void TestDate()
        {
            _oraTableCreator.Date();

            WriteAndVerify(
                "DateTime",
                TestTableCreator.DateTimeValue.ToString("yyyyMMdd HH:mm:ss"));

            ReadAndVerify("date", null, null, null);
        }

        [TestMethod]
        public void TestFloat()
        {
            _oraTableCreator.FloatColumn(0);

            WriteAndVerify(
                "Float",
                TestTableCreator.GetBinaryDoubleSqlValue());
            CheckThatDetailDoesNotExist(_schemaFileName, "Length");

            ReadAndVerify("float", null, 126, null);
        }

        [TestMethod]
        public void TestFloat20()
        {
            _oraTableCreator.FloatColumn(20);

            WriteAndVerify(
                "Float",
                "Prec", 20,
                TestTableCreator.GetBinaryFloatSqlValue());

            ReadAndVerify("float", null, 20, null);
        }

        [TestMethod]
        public void TestLong()
        {
            _oraTableCreator.LongColumn();

            WriteAndVerify(
                "OldText",
                TestTableCreator.GetLongTextSqlValue());
            CheckThatDetailDoesNotExist(_schemaFileName, "Length");

            ReadAndVerify("long", null, null, null);
        }

        [TestMethod]
        public void TestLongRaw()
        {
            _oraTableCreator.LongRawColumn();

            WriteAndVerify(
                "OldBlob",
                "i000000000000000.raw");
            var blobContent = File.ReadAllText(_blobFileName);
            blobContent.Should().Be(TestTableCreator.BlobValue);
            CheckThatDetailDoesNotExist(_schemaFileName, "Length");

            ReadAndVerify("long raw", null, null, null);
        }

        [TestMethod]
        public void TestNChar10()
        {
            _oraTableCreator.NCharColumn(10);

            WriteAndVerify(
                "NChar",
                "Length", 10,
                TestTableCreator.GetNCharSqlValue(DbContext));

            ReadAndVerify("nchar", 10, null, null);
        }

        [TestMethod]
        public void TestNClob()
        {
            _oraTableCreator.NClobColumn();

            WriteAndVerify(
                "NLongText",
                TestTableCreator.GetNLongTextSqlValue(DbContext));
            CheckThatDetailDoesNotExist(_schemaFileName, "Length");

            ReadAndVerify("nclob", null, null, null);
        }

        [TestMethod]
        public void TestNumber()
        {
            _oraTableCreator.Number();

            WriteAndVerify(
                "Dec",
                0, 0,
                TestTableCreator.GetFloatSqlValue());

            ReadAndVerify("number", null, 0, 0);
        }

        [TestMethod]
        public void TestNumberWithPrec()
        {
            _oraTableCreator.Number("23", null);

            WriteAndVerify(
                "Dec",
                23, 0,
                Convert.ToString((int)TestTableCreator.FloatValue));

            ReadAndVerify("number", null, 23, 0);
        }

        [TestMethod]
        public void TestNumberWithScale()
        {
            _oraTableCreator.Number("*", "15");

            WriteAndVerify(
                "Dec",
                0, 15,
                TestTableCreator.GetFloatSqlValue());

            ReadAndVerify("number", null, 0, 15);
        }

        [TestMethod]
        public void TestNumberWithPrecAndScale()
        {
            _oraTableCreator.Number("30", "15");

            WriteAndVerify(
                "Dec",
                30, 15,
                TestTableCreator.GetFloatSqlValue());

            ReadAndVerify("number", null, 30, 15);
        }

        [TestMethod]
        public void TestRaw()
        {
            _oraTableCreator.Raw(10);

            WriteAndVerify(
                "Raw",
                "Length", 10,
                TestTableCreator.RawValue);

            ReadAndVerify("raw", 10, null, null);
        }

        [TestMethod]
        public void TestRaw16()
        {
            _oraTableCreator.Raw(16);

            WriteAndVerify(
                "Raw",
                "Length", 16,
                TestTableCreator.RawValue);

            ReadAndVerify("raw", 16, null, null);
        }

        // Oracle doesn't have guids. Modify writerconversions.xml file to test guids.
        //[TestMethod]
        //public void TestGuid()
        //{
        //    _oraTableCreator.Guid();

        //    WriteAndVerify(
        //        "Guid",
        //        "Length", 16,
        //        TestTableCreator.GuidValue.ToString());

        //    ReadAndVerify("raw", 16, null, null);
        //}

        [TestMethod]
        public void TestRaw17()
        {
            _oraTableCreator.Raw(17);

            WriteAndVerify(
                "Raw",
                "Length", 17,
                TestTableCreator.RawValue);

            ReadAndVerify("raw", 17, null, null);
        }

        [TestMethod]
        public void TestTimestamp()
        {
            _oraTableCreator.Timestamp(0);

            WriteAndVerify(
                "DateTime2",
                "Scale", 6,
                TestTableCreator.DateTimeValue.ToString("yyyyMMdd HH:mm:ss"));

            ReadAndVerify("timestamp(6)", null, null, 6);
        }

        [TestMethod]
        public void TestTimestamp9()
        {
            _oraTableCreator.Timestamp(9);

            WriteAndVerify(
                "DateTime2",
                "Scale", 9,
                TestTableCreator.TimeStampValue.ToString("yyyyMMdd HH:mm:ss.fffffff"));

            ReadAndVerify("timestamp(9)", null, null, 9);
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