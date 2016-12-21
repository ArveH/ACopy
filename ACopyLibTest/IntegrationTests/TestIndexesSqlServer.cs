﻿using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.IntegrationTests
{
    [TestClass]
    public class TestIndexesSqlServer : TestIndexes
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());
            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSIndex()
        {
            TestIndex();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSIndexes_When_IndexExistsInAsysIndex()
        {
            TestIndexes_When_IndexExistsInAsysIndex();

            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeFalse("because we are not running Oracle");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSIndexes_When_IndexExistsInBothAagAndAsysIndex_Then_OnlyAAgAdded()
        {
            TestIndexes_When_IndexExistsInBothAagAndAsysIndex_Then_OnlyAAgAdded();

            DbSchema.IsIndex("i_" + TestTable, TestTable).Should().BeTrue("because index should exists");
            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeFalse("because index should only exist when running Oracle");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSFunctionBasedIndex()
        {
            TestFunctionBasedIndex();
            DbSchema.IsIndex("i_" + TestTable + "1", TestTable).Should().BeFalse("because function based index should not be created for Sql Server");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMSIndexes_When_SameIndexInAagIndexAndOnTable_Then_OnTableWins()
        {
            TestIndexes_When_SameIndexInAagIndexAndOnTable_Then_OnTableWins();
        }
    }
}