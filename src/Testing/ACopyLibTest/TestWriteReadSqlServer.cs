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
        private string _blobFileName;

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

        [TestMethod]
        public void TestDateTime()
        {
            _mssTableCreator.DateTimeColumn();

            WriteAndVerify(
                "<Type>DateTime</Type>",
                TestTableCreator.DateTimeValue.ToString("yyyyMMdd hh:mm:ss"));

            ReadAndVerify("datetime", null, null, null);
        }

        [TestMethod]
        public void TestDateTime2()
        {
            _mssTableCreator.DateTime2Column(0);

            WriteAndVerify(
                "<Type>DateTime2</Type>",
                "<Scale>7</Scale>",
                TestTableCreator.DateTimeValue.ToString("yyyyMMdd hh:mm:ss"));

            ReadAndVerify("datetime2", null, null, 7);
        }

        [TestMethod]
        public void TestDateTime2_5()
        {
            _mssTableCreator.DateTime2Column(5);

            WriteAndVerify(
                "<Type>DateTime2</Type>",
                "<Scale>5</Scale>",
                TestTableCreator.DateTimeValue.ToString("yyyyMMdd hh:mm:ss"));

            ReadAndVerify("datetime2", null, null, 5);
        }

        [TestMethod]
        public void TestDecimal()
        {
            _mssTableCreator.DecimalColumn(21,5);

            WriteAndVerify(
                "<Type>Dec</Type>",
                "<Prec>21</Prec>",
                "<Scale>5</Scale>",
                TestTableCreator.GetDecSqlValue());

            ReadAndVerify("decimal", null, 21, 5);
        }

        [TestMethod]
        public void TestNumeric()
        {
            _mssTableCreator.NumericColumn(21, 5);

            WriteAndVerify(
                "<Type>Dec</Type>",
                "<Prec>21</Prec>",
                "<Scale>5</Scale>",
                TestTableCreator.GetDecSqlValue());

            ReadAndVerify("decimal", null, 21, 5);
        }

        [TestMethod]
        public void TestFloat24()
        {
            _mssTableCreator.FloatColumn(24);

            WriteAndVerify(
                "<Type>Float</Type>",
                "<Prec>24</Prec>",
                TestTableCreator.GetBinaryFloatSqlValue());

            // OBS: Prec <= 24 will result in real and prec=24
            //      Prec > 24 will result in float(53)
            ReadAndVerify("real", null, 24, null);
        }

        [TestMethod]
        public void TestFloat()
        {
            _mssTableCreator.FloatColumn(0);

            WriteAndVerify(
                "<Type>Float</Type>",
                "<Prec>53</Prec>",
                TestTableCreator.GetBinaryFloatSqlValue());

            // OBS: Prec <= 24 will result in real and prec=24
            //      Prec > 24 will result in float(53)
            ReadAndVerify("float", null, 53, null);
        }

        [TestMethod]
        public void TestImage()
        {
            _mssTableCreator.ImageColumn();

            WriteAndVerify(
                "<Type>OldBlob</Type>",
                "i000000000000000.raw");
            var blobContent = File.ReadAllText(_blobFileName);
            blobContent.Should().Be(TestTableCreator.BlobValue);

            ReadAndVerify("image", null, null, null);
        }

        [TestMethod]
        public void TestInt()
        {
            _mssTableCreator.IntColumn();

            WriteAndVerify(
                "<Type>Int</Type>",
                TestTableCreator.GetIntSqlValue());

            ReadAndVerify("int", null, null, null);
        }

        [TestMethod]
        public void TestMoney()
        {
            _mssTableCreator.MoneyColumn();

            WriteAndVerify(
                "<Type>Money</Type>",
                TestTableCreator.GetMoneySqlValue());

            ReadAndVerify("money", null, null, null);
        }

        [TestMethod]
        public void TestSmallMoney()
        {
            _mssTableCreator.SmallMoneyColumn();

            WriteAndVerify(
                "<Type>SmallMoney</Type>",
                TestTableCreator.GetSmallMoneySqlValue());

            ReadAndVerify("smallmoney", null, null, null);
        }

        [TestMethod]
        public void TestSmallInt()
        {
            _mssTableCreator.SmallIntColumn();

            WriteAndVerify(
                "<Type>Int16</Type>",
                TestTableCreator.GetInt16SqlValue());

            ReadAndVerify("smallint", null, null, null);
        }

        [TestMethod]
        public void TestNChar10()
        {
            _mssTableCreator.NCharColumn(10);

            WriteAndVerify(
                "<Type>NChar</Type>",
                "<Length>10</Length>",
                TestTableCreator.NCharValue);

            ReadAndVerify("nchar", 10, null, null);
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
            string exptectedType,
            string exptectedNum1,
            string exptectedNum2,
            string expectedData)
        {
            WriteAndVerify(
                new List<string>()
                {
                    exptectedType,
                    exptectedNum2,
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
            if (expectedScale.HasValue) scale.Should().Be(expectedScale.Value);
        }

        #endregion
    }
}