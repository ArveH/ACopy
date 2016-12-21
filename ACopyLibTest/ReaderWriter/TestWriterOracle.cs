using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestWriterOracle: TestWriter
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
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

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_SimpleTable_Then_SchemaFileCreated()
        {
            TestWriter_When_SimpleTable_Then_SchemaFileCreated();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_SimpleTable_Then_DataFileCreated()
        {
            TestWriter_When_SimpleTable_Then_DataFileCreated();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_BlobTable()
        {
            TestWriter_When_BlobTable("utl_raw.cast_to_raw('A virtually very long line')");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_AllTypes()
        {
            TestWriter_When_AllTypes();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_NullValue()
        {
            TestWriter_When_NullValue();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_StringContainsQuote()
        {
            TestWriter_When_StringContainsQuote();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_StringContainsNewLine()
        {
            TestWriter_When_StringContainsNewLine();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_UseCompression()
        {
            TestWriter_When_UseCompression();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestMSWriter_When_BlobTableAndCompressedFlag()
        {
            TestWriter_When_BlobTableAndCompressedFlag("utl_raw.cast_to_raw('A virtually very long line')");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriter_When_CharColAndTrailingSpaces_Then_NoTrailingSpacesInDataFile()
        {
            TestWriter_When_CharColAndTrailingSpaces_Then_NoTrailingSpacesInDataFile();
        }
    }
}
