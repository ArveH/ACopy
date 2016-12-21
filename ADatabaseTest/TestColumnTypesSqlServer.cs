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

        private void TestStringColumn(ColumnType colType)
        {
            TestColumn(colType, 50, false, "' '", "Danish_Norwegian_CI_AS");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Varchar()
        {
            TestStringColumn(ColumnType.Varchar);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Char()
        {
            TestStringColumn(ColumnType.Char);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_String()
        {
            TestStringColumn(ColumnType.String);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_LongText()
        {
            TestStringColumn(ColumnType.LongText);
        }

        private void TestNumberColumn(ColumnType type)
        {
            TestColumn(type, 0, false, "0", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int()
        {
            TestNumberColumn(ColumnType.Int);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Bool()
        {
            TestNumberColumn(ColumnType.Bool);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int8()
        {
            TestNumberColumn(ColumnType.Int8);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int16()
        {
            TestNumberColumn(ColumnType.Int16);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Int64()
        {
            TestNumberColumn(ColumnType.Int64);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Money()
        {
            TestNumberColumn(ColumnType.Money);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Float()
        {
            TestNumberColumn(ColumnType.Float);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_DateTime()
        {
            TestColumn(ColumnType.DateTime, 0, false, "convert(datetime,'19000101',112)", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Guid()
        {
            TestColumn(ColumnType.Guid, 0, true, "", "");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSCreateTable_When_Raw()
        {
            TestColumn(ColumnType.Raw, 0, true, "", "");
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