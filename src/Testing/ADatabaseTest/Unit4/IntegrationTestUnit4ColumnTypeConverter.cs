using System;
using ACopyTestHelper;
using ADatabase;
using ADatabase.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class IntegrationTestUnit4ColumnTypeConverter
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

        private string GetDestinationTypeWhenOracleToACopy(string input, ref int length, ref int prec, ref int scale)
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.Unit4OracleWriterConversionsXml());
            return _columnTypeConverter.GetDestinationType(input, ref length, ref prec, ref scale);
        }

        [TestInitialize]
        public void Startup()
        {
            _length = 0;
            _prec = 0;
            _scale = 0;
        }

        [TestMethod]
        public void TestGetDestinationType_When_IllegalTypeName()
        {
            _length = 25;
            Action act = () => GetDestinationTypeWhenOracleToACopy("illegal_type(@Length)", ref _length, ref _prec, ref _scale);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type: 'illegal_type(@Length)', length=25, prec=0, scale=0");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Varchar2ToVarchar()
        {
            _length = 25;
            var destinationType = GetDestinationTypeWhenOracleToACopy("varchar2(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("varchar");
            _length.Should().Be(25);
        }

        [TestMethod]
        public void TestGetDestinationType_When_VarcharToVarchar()
        {
            _length = 25;
            var destinationType = GetDestinationTypeWhenOracleToACopy("varchar(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("varchar");
            _length.Should().Be(25);
        }

        [TestMethod]
        public void TestGetDestinationType_When_CharToVarchar()
        {
            _length = 25;
            var destinationType = GetDestinationTypeWhenOracleToACopy("char(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("varchar");
            _length.Should().Be(25);
        }

        [TestMethod]
        public void TestGetDestinationType_When_ClobToLongText()
        {
            var destinationType = GetDestinationTypeWhenOracleToACopy("clob", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("longtext");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToBool()
        {
            _prec = 1;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("bool");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt8()
        {
            _prec = 3;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int8");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt16()
        {
            _prec = 5;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int16");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt()
        {
            _prec = 15;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToInt64()
        {
            _prec = 20;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("int64");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney18_2()
        {
            _prec = 18;
            _scale = 2;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToMoney30_3()
        {
            _prec = 30;
            _scale = 3;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("money");
        }

        [TestMethod]
        public void TestGetDestinationType_When_AnyUnrecognizedNumber()
        {
            _prec = 32;
            _scale = 9;
            var destinationType = GetDestinationTypeWhenOracleToACopy("number(@Prec,@Scale)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_FloatToFloat()
        {
            var destinationType = GetDestinationTypeWhenOracleToACopy("float", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("float");
        }

        [TestMethod]
        public void TestGetDestinationType_When_DateToDateTime()
        {
            _length = 32;
            var destinationType = GetDestinationTypeWhenOracleToACopy("date", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("datetime");
        }

        [TestMethod]
        public void TestGetDestinationType_When_BlobToBlob()
        {
            var destinationType = GetDestinationTypeWhenOracleToACopy("blob", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("blob");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Guid()
        {
            _length = 16;
            var destinationType = GetDestinationTypeWhenOracleToACopy("raw(@Length)", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("guid");
        }

        [TestMethod]
        public void TestGetDestinationType_When_OldBlobToBlob()
        {
            var destinationType = GetDestinationTypeWhenOracleToACopy("oldblob", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("blob");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Int64ToNumber_20_0()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.Unit4OracleReaderConversionsXml());
            var destinationType = _columnTypeConverter.GetDestinationType("int64", ref _length, ref _prec, ref _scale);
            destinationType.Should().Be("number");
            _prec.Should().Be(20);
            _scale.Should().Be(0);
        }

    }
}