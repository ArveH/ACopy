using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestMiscSqlServer: TestMisc
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionHolderForTesting.GetSqlServerConnection());
            DbSchema = DbContext.PowerPlant.CreateDbSchema();
            Commands = DbContext.PowerPlant.CreateCommands();

            DbSchema.DropTable(TestTable);
            DeleteFiles();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_UseCompression_And_EmptyTable()
        {
            TestWriteRead_When_UseCompression_And_EmptyTable();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_UseCompression_And_StringWithQuote()
        {
            TestWriteRead_When_UseCompression_And_StringWithQuote();
        }
    }
}
