using ADatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestDoViewsOracle: TestDoViews
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
        public void TestOraDoViews_When_SimpleViewInAsysview_Then_IsViewTrue()
        {
            TestDoViews_When_SimpleViewInAsysview_Then_IsViewTrue();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraDoViews_When_ViewInAsysviewAndAagview_Then_AagviewIsUsed()
        {
            TestDoViews_When_ViewInAsysviewAndAagview_Then_AagviewIsUsed();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraDoViews_When_ViewContainNativeFunction_Then_ViewCreated()
        {
            string body = string.Format("select nvl(int_col, '' '') as new_col1 from {0}", TestTable);
            TestDoViews_When_ViewContainNativeFunction_Then_ViewCreated(body);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraDoViews_When_ViewContainAgrFunction_Then_ViewCreated()
        {
            TestDoViews_When_ViewContainAgrFunction_Then_ViewCreated();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraDoViews_When_ViewContainingQuotedName_Then_ViewCreated()
        {
            TestDoViews_When_ViewContainingQuotedName_Then_ViewCreated();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraDoViews_When_ViewContainingEmptyString()
        {
            TestDoViews_When_ViewContainingEmptyString();
        }
    }
}
