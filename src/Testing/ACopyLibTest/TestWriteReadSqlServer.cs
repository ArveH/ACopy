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
            _writer.WriteTable(TableName);
            var fileContent = File.ReadAllText(_schemaFileName);
            fileContent.Should().Contain("<Type>Int64</Type>");

            fileContent = File.ReadAllText(_dataFileName);
            fileContent.Should().Contain(TestTableCreator.GetInt64SqlValue());

            _reader.Read(new List<string>() { TableName }, out int tableCounter, out int errorCounter);


        }

        [TestMethod]
        public void TestBinary50()
        {
            _mssTableCreator.BinaryColumn();
            _writer.WriteTable(TableName);
            var fileContent = File.ReadAllText(_schemaFileName);
            fileContent.Should().Contain("<Type>Raw</Type>");
            fileContent.Should().Contain("<Length>50</Length>");

            fileContent = File.ReadAllText(_dataFileName);
            var expectedBase64Content = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(TestTableCreator.RawValue));
            fileContent.Should().Contain(expectedBase64Content);

            _reader.Read(new List<string>() { TableName }, out int tableCounter, out int errorCounter);
            DbSchema.GetRawColumnDefinition(TableName, "col1", out string type, out int length, out int prec, out int scale);
            type.Should().Be("binary");
            length.Should().Be(50);
        }
    }
}