using System;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestNullValuesSqlServer : TestNullValues
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

        private void CheckValue()
        {
            var val = Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            val.Should().BeOfType<DBNull>();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Int()
        {
            TestNullValue_When_Int(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Float()
        {
            TestNullValue_When_Float(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Varchar()
        {
            TestNullValue_When_Varchar(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_LongText()
        {
            TestNullValue_When_LongText(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Bool()
        {
            TestNullValue_When_Bool(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Int64()
        {
            TestNullValue_When_Int64(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Guid()
        {
            TestNullValue_When_Guid(CheckValue);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSNullValue_When_Raw()
        {
            TestNullValue_When_Raw(CheckValue);
        }
    }
}