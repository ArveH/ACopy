using System;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestNullValuesOracle : TestNullValues
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            ConversionFileForRead = "Resources/ACopyToUnit4Oracle.xml";
            ConversionFileForWrite = "Resources/Unit4OracleConversions.xml";
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

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Int()
        {
            TestNullValue_When_Int(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Float()
        {
            TestNullValue_When_Float(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Varchar()
        {
            TestNullValue_When_Varchar(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_LongText()
        {
            TestNullValue_When_LongText(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Bool()
        {
            TestNullValue_When_Bool(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Int64()
        {
            TestNullValue_When_Int64(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Guid()
        {
            TestNullValue_When_Guid(CheckValue);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraNullValue_When_Raw()
        {
            TestNullValue_When_Raw(CheckValue);
        }
    }
}