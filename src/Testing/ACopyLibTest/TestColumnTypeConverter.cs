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
            _columnTypeConverter.Initialize(GetConversionsXmlForNumberWithScaleAndPrec());
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
            _columnTypeConverter.Initialize(GetConversionsXmlForNumberWithScaleAndPrec());
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
            _columnTypeConverter.Initialize(GetConversionsXmlForNumberWithScaleAndPrec());
            int length = 22;
            int prec = 0;
            int scale = 2;
            var type = _columnTypeConverter.GetDestinationType("number", ref length, ref prec, ref scale);
            type.Should().Be("dec");
            prec.Should().Be(0);
            scale.Should().Be(2);
        }

        [TestMethod]
        public void TestFloat_When_BinaryFloat()
        {
            _columnTypeConverter.Initialize(GetConversionsXmlForFloat());
            int length = 4;
            int prec = 24;
            int scale = 0;
            var type = _columnTypeConverter.GetDestinationType("float(@Prec)", ref length, ref prec, ref scale);
            type.Should().Be("binaryfloat");
            length.Should().Be(0);
            prec.Should().Be(0);
            scale.Should().Be(0);
        }


        #region Private Helper functions
        public static string GetConversionsXmlForNumberWithScaleAndPrec()
        {
            return ConversionXmlHelper.GetHeadingXml() +
                "<TypeConversions From=\"DB\" To=\"ACopy\">\n" +
                //ConversionXmlHelper.GetOneTypeNoOperatorXml("number", "dec") +
                //ConversionXmlHelper.GetOneTypeNoOperatorXml("number(@Prec)", "dec(@Prec)") +
                ConversionXmlHelper.GetOneTypeNoOperatorXml("number(@Prec,@Scale)", "dec(@Prec,@Scale)") +
                "</TypeConversions>";
        }

        public static string GetConversionsXmlForFloat()
        {
            return ConversionXmlHelper.GetHeadingXml() +
                "<TypeConversions From=\"SqlServer\" To=\"Default\">\n" +
                "<Type Source=\"float(@Prec)\" Destination=\"binaryfloat\">\n" +
                "     <Prec Operator=\"=\">24</Prec>\n" +
                "</Type>\n" +
                "<Type Source=\"float(@Prec)\" Destination=\"binarydouble\">\n" +
                "     <Prec Operator=\"=\">53</Prec>\n" +
                "</Type>\n" +
                "<Type Source=\"float(@Prec)\" Destination=\"float(@Prec)\"></Type>\n" +
                "</TypeConversions>";
        }
        #endregion
    }
}