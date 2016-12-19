using ADatabase;
using AParser;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AParserTest
{
    [TestClass]
    public class TestOracleTranslator
    {
        private void TestDoFunctions(string input, string expected)
        {
            IASTNodeFactory nodeFactory = new ASTNodeFactory();
            IAParser parser = AParserFactory.CreateInstance(nodeFactory);
            ASTNodeList aNodes = parser.CreateNodeList(input);
            IATranslator translator = ATranslatorFactory.CreateInstance(DbType.Oracle, nodeFactory);
            ASTNodeList oraNodes = translator.Translate(aNodes);

            string result = oraNodes.ToString();
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestDayAdd_When_Ora()
        {
            TestDoFunctions("select DAYADD(3, day_col) as mycol", "select (3+ day_col) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestDayAdd_When_Ora_And_Expression()
        {
            TestDoFunctions("select DAYADD((3+4), day_col) as mycol", "select ((3+4)+ day_col) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestGuid2Str_When_Ora()
        {
            TestDoFunctions(
                "select GUID2STR(guid_col) as mycol",
                "select lower(substr(guid_col, 1, 8) || '-' || substr(guid_col, 9, 4) || '-' || substr(guid_col, 13, 4) || '-' || substr(guid_col, 17, 4) || '-' || substr(guid_col, 21, 12) ) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestGuid2Str_When_Ora_And_Expression()
        {
            TestDoFunctions(
                "select GUID2STR(func(str_col)) as mycol",
                "select lower(substr(func(str_col), 1, 8) || '-' || substr(func(str_col), 9, 4) || '-' || substr(func(str_col), 13, 4) || '-' || substr(func(str_col), 17, 4) || '-' || substr(func(str_col), 21, 12) ) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestMinDate_When_Ora()
        {
            TestDoFunctions("select MIN_DATE as mycol", "select to_date('19000101 00:00:00', 'yyyymmdd hh24:mi:ss') as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void Testmod_When_Ora()
        {
            TestDoFunctions("select MOD(3, 5) as mycol", "select mod(floor(3) , floor(5) ) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestMonthAdd_When_Ora()
        {
            TestDoFunctions("select MONTHADD(3, today) as mydate", "select add_months(today, 3) as mydate");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestToCounter_When_Ora()
        {
            TestDoFunctions("select TO_COUNTER('0') as mycol", "select cast(decode('0', ' ', 0.00, '0') as number(20) ) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestToInt_When_Ora()
        {
            TestDoFunctions("select TO_int('123') as mycol", "select to_number('123') as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestToFloat_When_Ora()
        {
            TestDoFunctions("select TO_FLOAT('123.123') as mycol", "select cast(decode('123.123', ' ', 0.00, '123.123') as number(30, 8) ) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestGetDate_When_Ora()
        {
            TestDoFunctions("select getdate() as mycol", "select sysdate as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestIfNull_When_Ora()
        {
            TestDoFunctions("select ifnull(somecol, 0) as mycol", "select nvl(somecol, 0) as mycol");
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestToChar_When_Ora()
        {
            TestDoFunctions("select TO_CHAR(123.123) as mycol", "select to_char(123.123) as mycol");
        }

    }
}
