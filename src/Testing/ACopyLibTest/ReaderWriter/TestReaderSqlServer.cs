using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestReaderSqlServer: TestReader
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());
            DbSchema = DbContext.PowerPlant.CreateDbSchema();
            Commands = DbContext.PowerPlant.CreateCommands();

            SchemaFile = TestTable + ".aschema";
            DataFile = TestTable + ".adata";
            DbSchema.DropTable(TestTable);
            DeleteFiles();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_SimpleTable()
        {
            TestReader_When_SimpleTable();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_CompressedTable()
        {
            TestReader_When_CompressedTable();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_RawColumn()
        {
            TestReader_When_RawColumn();
            var val = Commands.ExecuteScalar(string.Format("select convert(varchar(100), raw_col) as raw from {0}", TestTable));
            val.Should().Be("A long blob");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_RawColumn_And_Compressed()
        {
            TestReader_When_RawColumn_And_Compressed();
            var val = Commands.ExecuteScalar(string.Format("select convert(varchar(100), raw_col) as raw from {0}", TestTable));
            val.Should().Be("A long blob");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_Guid()
        {
            TestReader_When_Guid();
            var val = Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            val.ToString().Should().Be(TestGuid);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_AllTypes()
        {
            TestReader_When_AllTypes();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSReader_When_AllTypes_And_Compressed()
        {
            TestReader_When_AllTypes_And_Compressed();
        }
    }
}