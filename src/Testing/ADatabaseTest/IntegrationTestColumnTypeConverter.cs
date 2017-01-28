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

        [TestMethod]
        public void TestGetDestinationType_When_IllegalTypeName()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.OneTypeNoConstraints("varchar2(@Length)", "varchar(@Length)"));

            Action act = () => _columnTypeConverter.GetDestinationType("illegal_type(@Length)", 25, null, null);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type: 'illegal_type(@Length)', length=25, prec=, scale=");
        }

        [TestMethod]
        public void TestGetDestinationType_When_Varchar2ToVarchar()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.OneTypeNoConstraints("varchar2(@Length)", "varchar(@Length)"));

            var destinationType = _columnTypeConverter.GetDestinationType("varchar2(@Length)", 25, null, null);
            destinationType.Should().Be("varchar(25)");
        }

        [TestMethod]
        public void TestGetDestinationType_When_NumberToBool()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.FromNumberXml("bool", "1", "0"));

            var destinationType = _columnTypeConverter.GetDestinationType("number(@Prec,@Scale)", null, 1, 0);
            destinationType.Should().Be("bool");
        }

    }
}