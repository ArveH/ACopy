using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestCreateXmlFromTableSqlServer: TestCopyLibBase
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());
            TableName = "htablewithallcolumns";

            base.Setup();
            CreateTableWithAllColumns(true);
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