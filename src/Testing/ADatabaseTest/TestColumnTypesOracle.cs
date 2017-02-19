using ACopyTestHelper;
using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestColumnTypesOracle: TestColumnTypesBase
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

        [TestMethod]
        public void Testexpr()
        {
            
        }

    }
}