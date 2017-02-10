using System;
using System.IO;
using ADatabase;
using ADatabase.Exceptions;
using ADatabase.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestConvertOracleToACopy
    {
        private IColumnTypeConverter _columnTypeConverter;
        private int _length;
        private int _prec;
        private int _scale;

        [TestInitialize]
        public void Startup()
        {
            _length = 0;
            _prec = 0;
            _scale = 0;
            _columnTypeConverter = new ColumnTypeConverter(new XmlConversionsReader(new TypeDescriptionFactory(new TypeConstraintFactory(new TypeOperatorFactory()))));
            _columnTypeConverter.Initialize(File.ReadAllText("Resources/Unit4OracleWriterConversions.xml"));
        }

        private ColumnTypeName GetACopyType(string type, ref int len, ref int prec, ref int scale)
        {
            return _columnTypeConverter.GetDestinationType(type.ToOracleTypeWithParameters(), ref _length, ref _prec, ref _scale).ACopy2ColumnTypeName();
        }

        [TestMethod]
        public void TestVarchar2_Then_Varchar()
        {
            _length = 15;
            var acopyType = GetACopyType("VARCHAR2", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Varchar);
            _length.Should().Be(15);
        }

        [TestMethod]
        public void TestVarchar_Then_Varchar()
        {
            _length = 15;
            var acopyType = GetACopyType("VARCHAR", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Varchar);
            _length.Should().Be(15);
        }

        [TestMethod]
        public void TestChar_Then_Varchar()
        {
            _length = 15;
            var acopyType = GetACopyType("CHAR", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Varchar);
            _length.Should().Be(15);
        }

        [TestMethod]
        public void TestClob_Then_LongText()
        {
            var acopyType = GetACopyType("CLOB", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.LongText);
        }

        [TestMethod]
        public void TestInteger_Then_Int()
        {
            var acopyType = GetACopyType("INTEGER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Int);
        }

        [TestMethod]
        public void TestNumber1_0_Then_Bool()
        {
            _prec = 1;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Bool);
        }

        [TestMethod]
        public void TestNumber3_0_Then_Int8()
        {
            _prec = 3;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Int8);
        }

        [TestMethod]
        public void TestNumber5_0_Then_Int16()
        {
            _prec = 5;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Int16);
        }

        [TestMethod]
        public void TestNumber20_0_Then_Int64()
        {
            _prec = 20;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Int64);
        }

        [TestMethod]
        public void TestNumber15_0_Then_Int()
        {
            _prec = 15;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Int);
        }

        [TestMethod]
        public void TestNumber18_0_Then_Dec()
        {
            _prec = 18;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestNumber28_0_Then_int64()
        {
            _prec = 28;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Int64);
        }

        [TestMethod]
        public void TestNumber18_2_Then_Money()
        {
            _prec = 18;
            _scale = 2;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Money);
        }

        [TestMethod]
        public void TestNumber30_3_Then_Money()
        {
            _prec = 30;
            _scale = 3;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Money);
        }

        [TestMethod]
        public void TestNumber30_8_Then_Float()
        {
            _prec = 30;
            _scale = 8;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestNumber30_5_Then_Float()
        {
            _prec = 30;
            _scale = 5;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestNumber15_1_Then_Float()
        {
            _prec = 15;
            _scale = 1;
            var acopyType = GetACopyType("NUMBER", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestFloat_Then_Float()
        {
            var acopyType = GetACopyType("FLOAT", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestDate_Then_DateTime()
        {
            var acopyType = GetACopyType("DATE", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.DateTime);
        }

        [TestMethod]
        public void TestRaw16_Then_Guid()
        {
            _length = 16;
            var acopyType = GetACopyType("RAW", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestRaw17_Then_Guid()
        {
            _length = 17;
            var acopyType = GetACopyType("RAW", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestRaw32_Then_Guid()
        {
            _length = 32;
            var acopyType = GetACopyType("RAW", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestRaw34_Then_Guid()
        {
            _length = 34;
            var acopyType = GetACopyType("RAW", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestBlob_Then_Blob()
        {
            var acopyType = GetACopyType("BLOB", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Blob);
        }

        [TestMethod]
        public void TestLongRaw_Then_Blob()
        {
            var acopyType = GetACopyType("LONGRAW", ref _length, ref _prec, ref _scale);
            acopyType.Should().Be(ColumnTypeName.Blob);
        }

        [TestMethod]
        public void TestUnknownType_Then_Exception()
        {
            Action act = () => GetACopyType("UNKNOWNTYPE", ref _length, ref _prec, ref _scale);
            act.ShouldThrow<AColumnTypeException>().WithMessage("Illegal type: 'unknowntype', length=0, prec=0, scale=0");
        }

    }
}