using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestColumnTypesSqlServer: TestColumnTypes
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());
            PowerPlant = DbContext.PowerPlant;
            DbSchema = PowerPlant.CreateDbSchema();
            ColumnFactory = PowerPlant.CreateColumnFactory();

            TableName = "hmstestcolumntypes";
            DbSchema.DropTable(TableName);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        private void TestStringColumn(ColumnTypeName colType)
        {
            TestColumn(colType, 50, false, "' '", "Danish_Norwegian_CI_AS");
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
            TestStringColumn(ColumnTypeName.String);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_LongText()
        {
            TestStringColumn(ColumnTypeName.LongText);
        }

        private void TestNumberColumn(ColumnTypeName type)
        {
            TestColumn(type, 0, false, "0", "");
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
        public void TestMSCreateTable_When_DateTime()
        {
            TestColumn(ColumnTypeName.DateTime, 0, false, "convert(datetime,'19000101',112)", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Guid()
        {
            TestColumn(ColumnTypeName.Guid, 0, true, "", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Raw()
        {
            TestColumn(ColumnTypeName.Raw, 0, true, "", "");
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