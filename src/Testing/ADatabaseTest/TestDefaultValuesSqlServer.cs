using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    public class TestDefaultValuesSqlServer: TestDefaultValues
    {
        [TestInitialize]
        public override void Setup()
        {
            ConversionFileForRead = "Resources/Unit4MssReaderConversions.xml";
            ConversionFileForWrite = "Resources/Unit4MssWriterConversions.xml";
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());

            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_DateTime()
        {
            TestDateTimeDefauleValue("convert(datetime,'19000101',112)");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Date_And_MIN_DATE()
        {
            TestCreateTable_When_Date_And_MIN_DATE();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Date_And_MAX_DATE()
        {
            TestCreateTable_When_Date_And_MAX_DATE();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Date_And_MAX_DATE_Rounded()
        {
            TestCreateTable_When_Date_And_MAX_DATE_Rounded();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Date_And_TODAY()
        {
            TestCreateTable_When_Date_And_TODAY();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Date_And_NOW()
        {
            TestCreateTable_When_Date_And_NOW();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Guid_And_GUIDAsDefault()
        {
            TestCreateTable_When_Guid_And_GUIDAsDefault();
        }

    }
}