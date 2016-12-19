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
            DbContext = DbContextFactory.CreateOracleContext(ConnectionHolderForTesting.GetOracleConnection());
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

        private void TestStringColumn(ColumnType colType)
        {
            TestColumn(colType, 50, false, "' '", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Varchar()
        {
            TestStringColumn(ColumnType.Varchar);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Char()
        {
            TestStringColumn(ColumnType.Char);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_String()
        {
            TestStringColumn(ColumnType.String);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_LongText()
        {
            TestStringColumn(ColumnType.LongText);
        }

        private void TestNumberColumn(ColumnType type)
        {
            TestColumn(type, 0, false, "0", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int()
        {
            TestNumberColumn(ColumnType.Int);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Bool()
        {
            TestNumberColumn(ColumnType.Bool);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int8()
        {
            TestNumberColumn(ColumnType.Int8);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int16()
        {
            TestNumberColumn(ColumnType.Int16);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Int64()
        {
            TestNumberColumn(ColumnType.Int64);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Money()
        {
            TestNumberColumn(ColumnType.Money);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Float()
        {
            TestNumberColumn(ColumnType.Float);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_DateTime()
        {
            TestColumn(ColumnType.DateTime, 0, false, "to_date('19000101 00:00:00', 'yyyymmdd hh24:mi:ss')", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Guid()
        {
            TestColumn(ColumnType.Guid, 0, true, "", "");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraCreateTable_When_Raw()
        {
            TestColumn(ColumnType.Raw, 0, true, "", "");
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