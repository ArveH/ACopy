using System;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestColumnTypesOracle : TestColumnTypes
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_Varchar()
        {
            TestWriteRead_When_Varchar();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_LongText()
        {
            TestWriteRead_When_LongText();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_Bool()
        {
            TestWriteRead_When_Bool();
            var val = (short)Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            val.Should().Be(1);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_Int64()
        {
            TestWriteRead_When_Int64();
            var val = Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            Convert.ToInt64(val).Should().Be(123456789012345);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_Guid()
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Guid, "test_col", 16, true, "", "");
            CreateTestTable1Row3Columns1Value(col, "hextoraw('3f2504e04f8911d39a0c0305e82c3301')");
            WriteAndRead();
            VerifyType(col);
        }

        private void InsertIntoTableWith3Guids()
        {
            string tmp = string.Format("insert into {0} (id, guid1_col, guid2_col, val, guid3_col) values (9, hextoraw('1f2504e04f8911d39a0c0305e82c3301'), hextoraw('2f2504e04f8911d39a0c0305e82c3302'), 'control value', hextoraw('3f2504e04f8911d39a0c0305e82c3303'))", TestTable);
            Commands.ExecuteNonQuery(tmp);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraWriteRead_When_3Guids()
        {
            TestWriteRead_When_3Guids(InsertIntoTableWith3Guids);
        }
    }
}