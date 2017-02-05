using System;
using System.Xml;
using ADatabase;
using ADatabase.Exceptions;
using ADatabaseTest.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class IntegrationTestColumnTypeConverter
    {
        private readonly IColumnTypeConverter _columnTypeConverter =
            new ColumnTypeConverter(
                new XmlConversionsReader(
                    new TypeDescriptionFactory(
                        new TypeConstraintFactory(
                            new TypeOperatorFactory()))));

        private string GetDestinationType(string input, int length, int prec, int scale)
        {
            var newLength = length;
            var newPrec = prec;
            var newScale = scale;

            return _columnTypeConverter.GetDestinationType("illegal_type(@Length)", ref length, ref prec, ref scale);
        }

        [TestInitialize]
        public void Startup()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.Unit4OracleConversionsXml());
        }

        [TestMethod]
        public void TestGetDestinationType_When_IllegalTypeName()
        {
            Action act = () => GetDestinationType("illegal_type(@Length)", 25, 0, 0);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type: 'illegal_type(@Length)', length=25, prec=, scale=");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Varchar2ToVarchar()
        {
            var destinationType = GetDestinationType("varchar2(@Length)", 25, 0, 0);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_VarcharToVarchar()
        {
            var destinationType = GetDestinationType("varchar(@Length)", 25, 0, 0);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_CharToVarchar()
        {
            var destinationType = GetDestinationType("char(@Length)", 25, 0, 0);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_ClobToLongText()
        {
            var destinationType = GetDestinationType("clob", 0, 0, 0);
            destinationType.Should().Be("longtext");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToBool()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 1, 0);
            destinationType.Should().Be("bool");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt8()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 3, 0);
            destinationType.Should().Be("int8");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt16()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 5, 0);
            destinationType.Should().Be("int16");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 15, 0);
            destinationType.Should().Be("int");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt64()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 20, 0);
            destinationType.Should().Be("int64");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney18_2()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 18, 2);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney30_3()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 30, 3);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_AnyUnrecognizedNumber()
        {
            var destinationType = GetDestinationType("number(@Prec,@Scale)", 0, 32, 9);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_FloatToFloat()
        {
            var destinationType = GetDestinationType("float", 0, 0, 0);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_DateToDateTime()
        {
            var destinationType = GetDestinationType("date", 32, 0, 0);
            destinationType.Should().Be("datetime");
        }

        [TestMethod]
        public void TestGetDestinationType_When_BlobToRaw()
        {
            var destinationType = GetDestinationType("blob", 0, 0, 0);
            destinationType.Should().Be("raw");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Guid()
        {
            var destinationType = GetDestinationType("raw(@Length)", 16, 0, 0);
            destinationType.Should().Be("guid");
        }

        [TestMethod]
        public void TestGetDestinationType_When_LongRawToRaw()
        {
            var destinationType = GetDestinationType("long raw", 0, 0, 0);
            destinationType.Should().Be("raw");
        }
    }
}