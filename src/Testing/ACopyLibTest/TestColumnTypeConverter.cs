using ACopyTestHelper;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestColumnTypeConverter
    {
        private readonly IColumnTypeConverter _columnTypeConverter =
            new ColumnTypeConverter(
                new XmlConversionsReader(
                    new TypeDescriptionFactory(
                        new TypeConstraintFactory(
                            new TypeOperatorFactory()))));

        private string GetDestinationTypeWhenOracleToACopy(string input, ref int length, ref int prec, ref int scale)
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.Unit4OracleWriterConversionsXml());
            return _columnTypeConverter.GetDestinationType(input, ref length, ref prec, ref scale);
        }

        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void TestNumber_When_PrecAndScaleHasValues()
        {
            _columnTypeConverter.Initialize(GetConversionsXmlFor("number(@Prec,@Scale)", "dec(@Prec,@Scale)"));
            int length = 22;
            int prec = 19;
            int scale = 4;
            var type = _columnTypeConverter.GetDestinationType("number", ref length, ref prec, ref scale);
            type.Should().Be("dec");
            prec.Should().Be(19);
            scale.Should().Be(4);
        }

        [TestMethod]
        public void TestNumber_When_OnlyPrecHasValue()
        {
            _columnTypeConverter.Initialize(GetConversionsXmlFor("number(@Prec,@Scale)", "dec(@Prec,@Scale)"));
            int length = 22;
            int prec = 19;
            int scale = 0;
            var type = _columnTypeConverter.GetDestinationType("number", ref length, ref prec, ref scale);
            type.Should().Be("dec");
            prec.Should().Be(19);
            scale.Should().Be(0);
        }

        [TestMethod]
        public void TestNumber_When_OnlyScaleHasValue()
        {
            int length = 22;
            int prec = 0;
            int scale = 2;
            var type = _columnTypeConverter.GetDestinationType("number", ref length, ref prec, ref scale);
            type.Should().Be("dec");
            prec.Should().Be(0);
            scale.Should().Be(2);
        }

        #region Private Helper functions
        public static string GetConversionsXmlFor(string from, string to)
        {
            return ConversionXmlHelper.GetHeadingXml() +
                "<TypeConversions From=\"DB\" To=\"ACopy\">\n" +
                ConversionXmlHelper.GetOneTypeNoOperatorXml(from, to) +
                "</TypeConversions>";
        }
        #endregion
    }
}