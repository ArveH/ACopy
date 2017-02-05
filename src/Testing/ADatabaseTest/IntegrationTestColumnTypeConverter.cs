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

        [TestInitialize]
        public void Startup()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.Unit4OracleConversionsXml());
        }

        [TestMethod]
        public void TestGetDestinationType_When_IllegalTypeName()
        {
            Action act = () => _columnTypeConverter.GetDestinationType("illegal_type(@Length)", 25, null, null);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type: 'illegal_type(@Length)', length=25, prec=, scale=");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Varchar2ToVarchar()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("varchar2(@Length)", 25, null, null);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_VarcharToVarchar()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("varchar(@Length)", 25, null, null);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_CharToVarchar()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("char(@Length)", 25, null, null);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_ClobToLongText()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("clob", null, null, null);
            destinationType.Should().Be("longtext");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToBool()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 1, 0);
            destinationType.Should().Be("bool");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt8()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 3, 0);
            destinationType.Should().Be("int8");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt16()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 5, 0);
            destinationType.Should().Be("int16");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 15, 0);
            destinationType.Should().Be("int");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt64()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 20, 0);
            destinationType.Should().Be("int64");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney18_2()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 18, 2);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney30_3()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 30, 3);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_AnyUnrecognizedNumber()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 32, 9);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_FloatToFloat()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("float", null, null, null);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_DateToDateTime()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("date", 32, null, null);
            destinationType.Should().Be("datetime");
        }

        [TestMethod]
        public void TestGetDestinationType_When_BlobToRaw()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("blob", null, null, null);
            destinationType.Should().Be("raw");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Guid()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("raw(@Length)", 16, null, null);
            destinationType.Should().Be("guid");
        }

        [TestMethod]
        public void TestGetDestinationType_When_LongRawToRaw()
        {
            var destinationType = _columnTypeConverter.GetDestinationType("long raw", null, null, null);
            destinationType.Should().Be("raw");
        }
    }
}