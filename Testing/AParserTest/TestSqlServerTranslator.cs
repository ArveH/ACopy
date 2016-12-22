using ADatabase;
using AParser;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AParserTest
{
    [TestClass]
    public class TestSqlServerTranslator
    {
        private void TestDoFunctions(string input, string expected)
        {
            IASTNodeFactory nodeFactory = new ASTNodeFactory();
            IAParser parser = AParserFactory.CreateInstance(nodeFactory);
            ASTNodeList aNodes = parser.CreateNodeList(input);
            IATranslator translator = ATranslatorFactory.CreateInstance(DbType.SqlServer, nodeFactory);
            ASTNodeList msNodes = translator.Translate(aNodes);

            string result = msNodes.ToString();
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestDayAdd_When_MS()
        {
            TestDoFunctions("select DAYADD(3, day_col) as mycol", "select dateadd(dd, 3, day_col) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestDayAdd_When_MS_And_Expression()
        {
            TestDoFunctions("select DAYADD((3+4), day_col) as mycol", "select dateadd(dd, (3+4), day_col) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestGuid2Str_When_MS()
        {
            TestDoFunctions(
                "select GUID2STR(guid_col) as mycol",
                "select lower(guid_col) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestGuid2Str_When_MS_And_Expression()
        {
            TestDoFunctions(
                "select GUID2STR(func(str_col)) as mycol",
                "select lower(func(str_col)) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMinDate_When_MS()
        {
            TestDoFunctions("select MIN_DATE as mycol", "select convert(datetime, '19000101 00:00:00:000', 9) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void Testmod_When_MS()
        {
            TestDoFunctions("select MOD(3, 5) as mycol", "select (3) % (5) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMonthAdd_When_MS()
        {
            TestDoFunctions("select MONTHADD(3, today) as mydate", "select dateadd(mm, 3, today) as mydate");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestToCounter_When_MS()
        {
            TestDoFunctions("select TO_COUNTER('0') as mycol", "select convert(bigint, '0') as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestToInt_When_MS()
        {
            TestDoFunctions("select TO_int('123') as mycol", "select convert(int, '123') as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestToFloat_When_MS()
        {
            TestDoFunctions("select TO_FLOAT('123.123') as mycol", "select convert(dec(28, 8) , '123.123') as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestGetDate_When_MS()
        {
            TestDoFunctions("select getdate() as mycol", "select getdate() as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestIfNull_When_MS()
        {
            TestDoFunctions("select ifnull(somecol, 0) as mycol", "select isnull(somecol, 0) as mycol");
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestToChar_When_MS()
        {
            TestDoFunctions("select TO_CHAR(123.123) as mycol", "select convert(nvarchar, 123.123) as mycol");
        }

    }
}
