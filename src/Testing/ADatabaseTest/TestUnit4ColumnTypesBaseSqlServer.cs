using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestUnit4ColumnTypesBaseSqlServer: TestUnit4ColumnTypesBase
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

        private void TestStringColumn(ColumnTypeName colType)
        {
            TestColumn(colType, 50, 0, 0, false, "' '", "Danish_Norwegian_CI_AS");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Varchar()
        {
            TestStringColumn(ColumnTypeName.Varchar);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Char()
        {
            TestStringColumn(ColumnTypeName.Char);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_String()
        {
            TestStringColumn(ColumnTypeName.NVarchar);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_LongText()
        {
            TestColumn(ColumnTypeName.LongText, -1, 0, 0, false, "' '", "Danish_Norwegian_CI_AS");
        }

        private void TestNumberColumn(ColumnTypeName type)
        {
            TestColumn(type, 0, 0, 0, false, "0", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int()
        {
            TestNumberColumn(ColumnTypeName.Int);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Bool()
        {
            TestNumberColumn(ColumnTypeName.Bool);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int8()
        {
            TestNumberColumn(ColumnTypeName.Int8);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int16()
        {
            TestNumberColumn(ColumnTypeName.Int16);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int64()
        {
            TestNumberColumn(ColumnTypeName.Int64);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Money()
        {
            TestNumberColumn(ColumnTypeName.Money);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Float()
        {
            TestNumberColumn(ColumnTypeName.Float);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Guid()
        {
            TestColumn(ColumnTypeName.Guid, 16, 0, 0, true, "", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Blob()
        {
            TestColumn(ColumnTypeName.Blob, -1, 0, 0, true, "", "");
        }
    }
}