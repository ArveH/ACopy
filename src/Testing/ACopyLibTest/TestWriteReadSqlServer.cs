using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ACopyLib.Reader;
using ACopyLib.Writer;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestWriteReadSqlServer: TestCopyLibBase
    {
        private MssTableCreator _mssTableCreator;
        private IAWriter _writer;
        private IAReader _reader;
        private string _schemaFileName;
        private string _dataFileName;

        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());
            _mssTableCreator = new MssTableCreator(DbContext);
            TableName = _mssTableCreator.TableName;

            _writer = AWriterFactory.CreateInstance(DbContext);
            _writer.Directory = ".\\";
            _reader = AReaderFactory.CreateInstance(DbContext);
            _reader.Directory = ".\\";

            _schemaFileName = $@".\{TableName}.{_writer.SchemaFileSuffix}";
            _dataFileName = $@".\{TableName}.{_writer.DataFileSuffix}";

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
        public void TestBigInt()
        {
            _mssTableCreator.BigIntColumn();

            WriteAndVerify(
                "<Type>Int64</Type>",
                TestTableCreator.GetInt64SqlValue());

            ReadAndVerify("bigint", null, null, null);
        }

        [TestMethod]
        public void TestBinary50()
        {
            _mssTableCreator.BinaryColumn();

            WriteAndVerify(
                "<Type>Raw</Type>",
                "<Length>50</Length>",
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        TestTableCreator.RawValue)));

            ReadAndVerify("binary", 50, null, null);
        }

        [TestMethod]
        public void TestBit()
        {
            _mssTableCreator.BitColumn();

            WriteAndVerify(
                "<Type>Bool</Type>",
                TestTableCreator.GetBoolSqlValue());

            ReadAndVerify("bit", null, null, null);
        }

        [TestMethod]
        public void TestChar10()
        {
            _mssTableCreator.CharColumn(10);

            WriteAndVerify(
                "<Type>Char</Type>",
                "<Length>10</Length>",
                TestTableCreator.GetCharSqlValue());

            ReadAndVerify("char", 10, null, null);
        }

        [TestMethod]
        public void TestDate()
        {
            _mssTableCreator.DateColumn();

            WriteAndVerify(
                "<Type>Date</Type>",
                TestTableCreator.DateValue.ToString("yyyyMMdd"));

            ReadAndVerify("date", null, null, null);
        }

        #region Private

        private void WriteAndVerify(
            string exptectedType,
            string expectedData)
        {
            WriteAndVerify(
                new List<string>() {exptectedType},
                expectedData);
        }

        private void WriteAndVerify(
            string exptectedType,
            string exptectedNum1,
            string expectedData)
        {
            WriteAndVerify(
                new List<string>()
                {
                    exptectedType,
                    exptectedNum1
                },
                expectedData);
        }

        private void WriteAndVerify(
            List<string> exptectedSchemaStuff,
            string expectedData)
        {
            _writer.WriteTable(TableName);
            var fileContent = File.ReadAllText(_schemaFileName);
            foreach (var str in exptectedSchemaStuff)
            {
                fileContent.Should().Contain(str);
            }

            fileContent = File.ReadAllText(_dataFileName);
            fileContent.Should().Contain(expectedData);
        }

        private void ReadAndVerify(
            string expectedType,
            int? expectedLength,
            int? expectedPrec,
            int? expectedScale)
        {
            _reader.Read(new List<string>() { TableName }, out int tableCounter, out int errorCounter);
            DbSchema.GetRawColumnDefinition(TableName, "col1", out string type, out int length, out int prec, out int scale);

            type.Should().Be(expectedType);
            if (expectedLength.HasValue) length.Should().Be(expectedLength.Value);
            if (expectedPrec.HasValue) prec.Should().Be(expectedPrec.Value);
            if (expectedScale.HasValue) length.Should().Be(expectedScale.Value);
        }

        #endregion
    }
}