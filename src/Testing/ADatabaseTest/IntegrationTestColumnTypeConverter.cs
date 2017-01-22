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
        [TestMethod]
        public void TestGetDestinationType_When_IllegalTypeName()
        {
            ITypeOperatorFactory typeOperatorFactory = new TypeOperatorFactory();
            ITypeConstraintFactory typeConstraintFactory = new TypeConstraintFactory(typeOperatorFactory);
            ITypeDescriptionFactory typeDescriptionFactory = new TypeDescriptionFactory(typeConstraintFactory);
            IXmlConversionsReader xmlConversionsReader = new XmlConversionsReader(typeDescriptionFactory);
            IColumnTypeConverter columnTypeConverter = new ColumnTypeConverter(xmlConversionsReader);
            columnTypeConverter.Initialize(ConversionXmlHelper.OneTypeNoConstraints("varchar2", "varchar"));

            Action act = () => columnTypeConverter.GetDestinationType("illegal_type", null, null, null);
            act.ShouldThrow<AColumnTypeException>()
                .WithMessage("Illegal type name 'illegal_type', length=, prec=, scale=");
        }

    }
}