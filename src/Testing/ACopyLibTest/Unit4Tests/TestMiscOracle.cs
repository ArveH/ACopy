using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.Unit4Tests
{
    [TestClass]
    public class TestMiscOracle: TestMisc
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            DbSchema = DbContext.PowerPlant.CreateDbSchema();
            Commands = DbContext.PowerPlant.CreateCommands();
            ConversionFileForRead = "Resources/Unit4OracleReaderConversions.xml";
            ConversionFileForWrite = "Resources/Unit4OracleWriterConversions.xml";

            DbSchema.DropTable(TestTable);
            DeleteFiles();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_UseCompression_And_EmptyTable()
        {
            TestWriteRead_When_UseCompression_And_EmptyTable();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_UseCompression_And_StringWithQuote()
        {
            TestWriteRead_When_UseCompression_And_StringWithQuote();
        }
    }
}
