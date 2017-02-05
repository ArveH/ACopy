using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestColumnTypesOracle: TestColumnTypes
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            PowerPlant = DbContext.PowerPlant;
            DbSchema = PowerPlant.CreateDbSchema();
            ColumnFactory = PowerPlant.CreateColumnFactory();

            TableName = "horatestcolumntypes";
            DbSchema.DropTable(TableName);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        private void TestStringColumn(ColumnTypeName colType)
        {
            TestColumn(colType, 50, 0, 0, false, "' '", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Varchar()
        {
            TestStringColumn(ColumnTypeName.Varchar);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Char()
        {
            TestStringColumn(ColumnTypeName.Char);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_String()
        {
            TestStringColumn(ColumnTypeName.String);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_LongText()
        {
            TestStringColumn(ColumnTypeName.LongText);
        }

        private void TestNumberColumn(ColumnTypeName type, int prec, int scale)
        {
            TestColumn(type, 0, prec, scale, false, "0", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int()
        {
            TestNumberColumn(ColumnTypeName.Int, 15, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Bool()
        {
            TestNumberColumn(ColumnTypeName.Bool, 1, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int8()
        {
            TestNumberColumn(ColumnTypeName.Int8, 3, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int16()
        {
            TestNumberColumn(ColumnTypeName.Int16, 5, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int64()
        {
            TestNumberColumn(ColumnTypeName.Int64, 20, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Money()
        {
            TestNumberColumn(ColumnTypeName.Money, 18, 3);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Float()
        {
            TestNumberColumn(ColumnTypeName.Float, 30, 8);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_DateTime()
        {
            TestColumn(ColumnTypeName.DateTime, 0, 0, 0, false, "to_date('19000101 00:00:00', 'yyyymmdd hh24:mi:ss')", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Guid()
        {
            TestColumn(ColumnTypeName.Guid, 16, 0, 0, true, "", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Raw()
        {
            TestColumn(ColumnTypeName.Blob, 0, 0, 0, true, "", "");
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