using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestCreateXmlFromTableOracle : TestCopyLibBase
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            TableName = "htablewithallcolumns";

            base.Setup();
            CreateTableWithAllColumns(false);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        public void TestSomething()
        {
            true.Should().Be(true);
        }
    }
}