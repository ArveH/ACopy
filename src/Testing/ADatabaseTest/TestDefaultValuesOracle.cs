using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestDefaultValuesOracle: TestDefaultValues
    {
        public override void Setup()
        {
            ConversionFileForRead = "Resources/Unit4OracleReaderConversions.xml";
            ConversionFileForWrite = "Resources/Unit4OracleWriterConversions.xml";
            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Date_And_MIN_DATE()
        {
            TestCreateTable_When_Date_And_MIN_DATE();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Date_And_MAX_DATE()
        {
            TestCreateTable_When_Date_And_MAX_DATE();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Date_And_MAX_DATE_Rounded()
        {
            TestCreateTable_When_Date_And_MAX_DATE_Rounded();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Date_And_TODAY()
        {
            TestCreateTable_When_Date_And_TODAY();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Date_And_NOW()
        {
            TestCreateTable_When_Date_And_NOW();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Guid_And_GUIDAsDefault()
        {
            TestCreateTable_When_Guid_And_GUIDAsDefault();
        }

    }
}