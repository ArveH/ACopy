using System.Collections.Generic;
using System.IO;
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
            _mssTableCreator.SqlServerBigIntColumn();
            _writer.WriteTable(TableName);
            int tableCounter;
            int errorCounter;
            _reader.Read(new List<string>() {TableName}, out tableCounter, out errorCounter);

            var fileContent = File.ReadAllText(_schemaFileName);
            fileContent.Should().Contain("<Type>Int64</Type>");

            fileContent = File.ReadAllText(_dataFileName);
            fileContent.Should().Contain(TestTableCreator.GetInt64SqlValue());
        }
    }
}