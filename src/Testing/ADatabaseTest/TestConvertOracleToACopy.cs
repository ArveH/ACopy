using System;
using System.IO;
using ADatabase;
using ADatabase.Exceptions;
using ADatabase.Extensions;
using ADatabase.Oracle.Columns;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestConvertOracleToACopy
    {
        private IColumnTypeConverter _columnTypeConverter;

        [TestInitialize]
        public void Startup()
        {
            _columnTypeConverter = new ColumnTypeConverter(new XmlConversionsReader(new TypeDescriptionFactory(new TypeConstraintFactory(new TypeOperatorFactory()))));
            _columnTypeConverter.Initialize(File.ReadAllText("Resources/Unit4OracleConversions.xml"));
        }

        private ColumnTypeName GetACopyType(string type, int len=0, int prec=0, int scale=0)
        {
            var newLength = len;
            var newPrec = prec;
            var newScale = scale;

            return _columnTypeConverter.GetDestinationType(type, ref newLength, ref newPrec, ref newScale).Oracle2ColumnTypeName();
        }

        [TestMethod]
        public void TestVarchar2_Then_Varchar()
        {
            var acopyType = GetACopyType("VARCHAR2", 15);
            acopyType.Should().Be(ColumnTypeName.Varchar);
        }

        [TestMethod]
        public void TestVarchar_Then_Varchar()
        {
            var acopyType = GetACopyType("VARCHAR", 15);
            acopyType.Should().Be(ColumnTypeName.Varchar);
        }

        [TestMethod]
        public void TestChar_Then_Varchar()
        {
            var acopyType = GetACopyType("CHAR", 15);
            acopyType.Should().Be(ColumnTypeName.Varchar);
        }

        [TestMethod]
        public void TestClob_Then_LongText()
        {
            var acopyType = GetACopyType("CLOB");
            acopyType.Should().Be(ColumnTypeName.LongText);
        }

        [TestMethod]
        public void TestInteger_Then_Int()
        {
            var acopyType = GetACopyType("INTEGER");
            acopyType.Should().Be(ColumnTypeName.Int);
        }

        [TestMethod]
        public void TestNumber1_0_Then_Bool()
        {
            var acopyType = GetACopyType("NUMBER", 0, 1);
            acopyType.Should().Be(ColumnTypeName.Bool);
        }

        [TestMethod]
        public void TestNumber3_0_Then_Int8()
        {
            var acopyType = GetACopyType("NUMBER", 0, 3);
            acopyType.Should().Be(ColumnTypeName.Int8);
        }

        [TestMethod]
        public void TestNumber5_0_Then_Int16()
        {
            var acopyType = GetACopyType("NUMBER", 0, 5);
            acopyType.Should().Be(ColumnTypeName.Int16);
        }

        [TestMethod]
        public void TestNumber20_0_Then_Int64()
        {
            var acopyType = GetACopyType("NUMBER", 0, 20);
            acopyType.Should().Be(ColumnTypeName.Int64);
        }

        [TestMethod]
        public void TestNumber15_0_Then_Int()
        {
            var acopyType = GetACopyType("NUMBER", 0, 15);
            acopyType.Should().Be(ColumnTypeName.Int);
        }

        [TestMethod]
        public void TestNumber18_0_Then_Int()
        {
            var acopyType = GetACopyType("NUMBER", 0, 18);
            acopyType.Should().Be(ColumnTypeName.Int);
        }

        [TestMethod]
        public void TestNumber28_0_Then_Int()
        {
            var acopyType = GetACopyType("NUMBER", 0, 28);
            acopyType.Should().Be(ColumnTypeName.Int);
        }

        [TestMethod]
        public void TestNumber18_2_Then_Money()
        {
            var acopyType = GetACopyType("NUMBER", 0, 18, 2);
            acopyType.Should().Be(ColumnTypeName.Money);
        }

        [TestMethod]
        public void TestNumber30_3_Then_Money()
        {
            var acopyType = GetACopyType("NUMBER", 0, 30, 3);
            acopyType.Should().Be(ColumnTypeName.Money);
        }

        [TestMethod]
        public void TestNumber30_8_Then_Float()
        {
            var acopyType = GetACopyType("NUMBER", 0, 30, 8);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestNumber30_5_Then_Float()
        {
            var acopyType = GetACopyType("NUMBER", 0, 30, 5);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestNumber15_1_Then_Float()
        {
            var acopyType = GetACopyType("NUMBER", 0, 15, 1);
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestFloat_Then_Float()
        {
            var acopyType = GetACopyType("FLOAT");
            acopyType.Should().Be(ColumnTypeName.Float);
        }

        [TestMethod]
        public void TestDate_Then_DateTime()
        {
            var acopyType = GetACopyType("DATE");
            acopyType.Should().Be(ColumnTypeName.DateTime);
        }

        [TestMethod]
        public void TestRaw16_Then_Guid()
        {
            var acopyType = GetACopyType("RAW", 16);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestRaw17_Then_Guid()
        {
            var acopyType = GetACopyType("RAW", 17);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestRaw32_Then_Guid()
        {
            var acopyType = GetACopyType("RAW", 32);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestRaw34_Then_Guid()
        {
            var acopyType = GetACopyType("RAW", 34);
            acopyType.Should().Be(ColumnTypeName.Guid);
        }

        [TestMethod]
        public void TestBlob_Then_Raw()
        {
            var acopyType = GetACopyType("BLOB");
            acopyType.Should().Be(ColumnTypeName.Raw);
        }

        [TestMethod]
        public void TestLongRaw_Then_Raw()
        {
            var acopyType = GetACopyType("LONG RAW");
            acopyType.Should().Be(ColumnTypeName.Raw);
        }

        [TestMethod]
        public void TestUnknownType_Then_Exception()
        {
            Action act = () => GetACopyType("UNKNOWNTYPE");
            act.ShouldThrow<ADatabaseException>().WithMessage("Copy program doesn't handle columns of type UNKNOWNTYPE");
        }

    }
}