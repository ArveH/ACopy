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
            _columnTypeConverter.Initialize(ConversionXmlHelper.OneTypeNoConstraints("varchar2", "varchar"));

            Action act = () => _columnTypeConverter.GetDestinationType("illegal_type", null, null, null);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type name 'illegal_type', length=, prec=, scale=");
        }

        [TestMethod]
        public void TestGetDestinationType_When_varchar2()
        {
            _columnTypeConverter.Initialize(ConversionXmlHelper.OneTypeNoConstraints("varchar2", "varchar"));

            var destinationType = _columnTypeConverter.GetDestinationType("varchar2", 25, null, null);
            destinationType.Should().Be("varchar(25)");
        }

    }
}