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
        private int _length;
        private int _prec;
        private int _scale;

        private readonly IColumnTypeConverter _columnTypeConverter =
            new ColumnTypeConverter(
                new XmlConversionsReader(
                    new TypeDescriptionFactory(
                        new TypeConstraintFactory(
                            new TypeOperatorFactory()))));

        private string GetDestinationType(string input, ref int length, ref int prec, ref int scale)
        {
            return _columnTypeConverter.GetDestinationType(input, ref length, ref prec, ref scale);
        }

        [TestInitialize]
        public void Startup()
        {
            _length = 0;
            _prec = 0;
            _scale = 0;
            _columnTypeConverter.Initialize(ConversionXmlHelper.Unit4OracleConversionsXml());
        }

        [TestMethod]
        public void TestGetDestinationType_When_IllegalTypeName()
        {
            _length = 25;
            Action act = () => GetDestinationType("illegal_type(@Length)", ref _length, ref _prec, ref _scale);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type: 'illegal_type(@Length)', length=25, prec=0, scale=0");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Varchar2ToVarchar()
        {
            _length = 25;
            var destinationType = GetDestinationType("varchar2(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("varchar");
            _length.Should().Be(25);
        }

        [TestMethod]
        public void TestGetDestinationType_When_VarcharToVarchar()
        {
            _length = 25;
            var destinationType = GetDestinationType("varchar(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("varchar");
            _length.Should().Be(25);
        }

        [TestMethod]
        public void TestGetDestinationType_When_CharToVarchar()
        {
            _length = 25;
            var destinationType = GetDestinationType("char(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("varchar");
            _length.Should().Be(25);
        }

        [TestMethod]
        public void TestGetDestinationType_When_ClobToLongText()
        {
            var destinationType = GetDestinationType("clob", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("longtext");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToBool()
        {
            _prec = 1;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("bool");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt8()
        {
            _prec = 3;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int8");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt16()
        {
            _prec = 5;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int16");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt()
        {
            _prec = 15;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt64()
        {
            _prec = 20;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int64");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney18_2()
        {
            _prec = 18;
            _scale = 2;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney30_3()
        {
            _prec = 30;
            _scale = 3;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_AnyUnrecognizedNumber()
        {
            _prec = 32;
            _scale = 9;
            var destinationType = GetDestinationType("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_FloatToFloat()
        {
            var destinationType = GetDestinationType("float", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_DateToDateTime()
        {
            _length = 32;
            var destinationType = GetDestinationType("date", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("datetime");
        }

        [TestMethod]
        public void TestGetDestinationType_When_BlobToRaw()
        {
            var destinationType = GetDestinationType("blob", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("raw");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Guid()
        {
            _length = 16;
            var destinationType = GetDestinationType("raw(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("guid");
        }

        [TestMethod]
        public void TestGetDestinationType_When_LongRawToRaw()
        {
            var destinationType = GetDestinationType("long raw", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("raw");
        }
    }
}