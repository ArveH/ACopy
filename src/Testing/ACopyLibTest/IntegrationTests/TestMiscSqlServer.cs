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
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());
            DbSchema = DbContext.PowerPlant.CreateDbSchema();
            Commands = DbContext.PowerPlant.CreateCommands();
            ConversionFileForRead = "Resources/Unit4MssReaderConversions.xml";
            ConversionFileForWrite = "Resources/Unit4MssWriterConversions.xml";

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
