using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestColumnTypesSqlServer : TestColumnTypes
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionHolderForTesting.GetSqlServerConnection());
            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_Varchar()
        {
            TestWriteRead_When_Varchar();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_LongText()
        {
            TestWriteRead_When_LongText();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_Bool()
        {
            TestWriteRead_When_Bool();
            var val = (bool)Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            val.Should().BeTrue();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_Int64()
        {
            TestWriteRead_When_Int64();
            var val = Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            val.Should().Be(123456789012345);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_Guid()
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Guid, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col, "'3f2504e0-4f89-11d3-9a0c-0305e82c3301'");
            WriteAndRead();
            VerifyType(col);
        }

        private void InsertIntoTableWith3Guids()
        {
            string tmp = string.Format("insert into {0} (id, guid1_col, guid2_col, val, guid3_col) values (9, '1f2504e0-4f89-11d3-9a0c-0305e82c3301', '2f2504e0-4f89-11d3-9a0c-0305e82c3302', 'control value', '3f2504e0-4f89-11d3-9a0c-0305e82c3303')", TestTable);
            Commands.ExecuteNonQuery(tmp);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSWriteRead_When_3Guids()
        {
            TestWriteRead_When_3Guids(InsertIntoTableWith3Guids);
        }
    }
}