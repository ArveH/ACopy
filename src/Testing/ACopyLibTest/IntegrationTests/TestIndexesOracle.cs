using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestIndexesOracle : TestIndexes
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

        [TestMethod, TestCategory("Oracle")]
        public void TestOraIndex()
        {
            TestIndex();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraIndexes_When_IndexExistsInAsysIndex()
        {
            TestIndexes_When_IndexExistsInAsysIndex();

            DbSchema.IsIndex("i_" + TestTable, TestTable).Should().BeTrue("because index should exists");
            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeTrue("because additional index exists in asysIndex and we are running Oracle");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraIndexes_When_IndexExistsInAsysIndex_But_U4IndexesIsNotSet()
        {
            TestIndexes_When_IndexExistsInAsysIndex_But_U4IndexesIsNotSet();

            DbSchema.IsIndex("i_" + TestTable, TestTable).Should().BeTrue("because index should exists");
            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeFalse("because additional indexex in asysIndex are ignored, and we are running Oracle");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraIndexes_When_IndexExistsInBothAagAndAsysIndex_Then_OnlyAAgAdded()
        {
            TestIndexes_When_IndexExistsInBothAagAndAsysIndex_Then_OnlyAAgAdded();

            DbSchema.IsIndex("i_" + TestTable, TestTable).Should().BeTrue("because index should exists");
            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeTrue("because additional index exists when running Oracle");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraFunctionBasedIndex()
        {
            TestFunctionBasedIndex();
            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeTrue("because function based index should be created");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOraIndexes_When_SameIndexInAagIndexAndOnTable_Then_OnTableWins()
        {
            TestIndexes_When_SameIndexInAagIndexAndOnTable_Then_OnTableWins();
        }
    }
}