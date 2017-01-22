using System;
using System.Xml;
using ADatabase;
using ADatabase.Exceptions;
using ADatabaseTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace ADatabaseTest
{
    [TestClass]
    public class TestXmlConversionsReader
    {
        private IXmlConversionsReader _xmlConversionsReader;

        [TestInitialize]
        public void Startup()
        {
            ITypeOperatorFactory typeOperatorFactory = new TypeOperatorFactory();
            ITypeConstraintFactory typeConstraintFactory = new TypeConstraintFactory(typeOperatorFactory);
            ITypeDescriptionFactory typeDescriptionFactory = new TypeDescriptionFactory(typeConstraintFactory);
            _xmlConversionsReader = new XmlConversionsReader(typeDescriptionFactory);
        }

        [TestMethod]
        public void TestGetRootNode_When_IllegalXml()
        {
            Action act = () => _xmlConversionsReader.GetRootNode("<illegal xml>");
            act.ShouldThrow<XmlException>().WithMessage("Error when reading conversion XML");
        }

        [TestMethod]
        public void TestGetRootNode_When_LegalXmlButIncorrectRootElement()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.LegalXmlButIncorrectRootElement());
            act.ShouldThrow<XmlException>().WithMessage("Can't find root element 'TypeConversions'");
        }

        [TestMethod]
        public void TestGetRootNode_When_FromAttributeMissing()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.FromAttributeMissing());
            act.ShouldThrow<XmlException>().WithMessage("Error with attribute 'From' for 'TypeConversions'");
        }

        [TestMethod]
        public void TestGetRootNode_When_ToAttributeMissing()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.ToAttributeBlank());
            act.ShouldThrow<XmlException>().WithMessage("Error with attribute 'To' for 'TypeConversions'");
        }

        [TestMethod]
        public void TestGetRootNode_When_LegalXmlButNoConversions()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.LegalRootButNoConversions());
            act.ShouldThrow<XmlException>().WithMessage("No conversions found");
        }

        [TestMethod]
        public void TestGetSourceSystem()
        {
            var rootNode = _xmlConversionsReader.GetRootNode(ConversionXmlHelper.LegalRootOneVarcharColumn());
            var sourceSystem = _xmlConversionsReader.GetSourceSystem(rootNode);
            sourceSystem.Should().Be(DatabaseSystemNames.Oracle);
        }

        [TestMethod]
        public void TestGetDestinationSystem()
        {
            var rootNode = _xmlConversionsReader.GetRootNode(ConversionXmlHelper.LegalRootOneVarcharColumn());
            var sourceSystem = _xmlConversionsReader.GetDestinationSystem(rootNode);
            sourceSystem.Should().Be(DatabaseSystemNames.ACopy);
        }

        [TestMethod]
        public void TestGetColumnTypeDescription_When_NameAttributeMissing()
        {
            Action act = () => _xmlConversionsReader.GetColumnTypeDescription(ConversionXmlHelper.NameAttributeMissingForType());
            act.ShouldThrow<XmlException>().WithMessage("Error with attribute 'Name' for 'Type'");
        }

        [TestMethod]
        public void TestGetColumnTypeDescription_When_ToAttributeMissing()
        {
            Action act = () => _xmlConversionsReader.GetColumnTypeDescription(ConversionXmlHelper.ToAttributeMissingForType());
            act.ShouldThrow<XmlException>().WithMessage("Error with attribute 'To' for 'Type'");
        }

        [TestMethod]
        public void TestGetColumnTypeDescription_When_IllegalTypeDetail()
        {
            Action act = () => _xmlConversionsReader.GetColumnTypeDescription(ConversionXmlHelper.IllegatTypeDetail());
            act.ShouldThrow<XmlException>().WithMessage("Illegal type detail 'Illegal' for type 'number'");
        }

        [TestMethod]
        public void TestGetColumnTypeDescription_When_OracleVarchar2()
        {
            var colDesc = _xmlConversionsReader.GetColumnTypeDescription(ConversionXmlHelper.OracleVarchar2());
            colDesc.TypeName.Should().Be("varchar2", "because type name is varchar");
            colDesc.ConvertTo.Should().Be("varchar", "because varchar2 should be varchar");
        }

        [TestMethod]
        public void TestGetColumnTypeDescription_When_OracleBool()
        {
            var colDesc = _xmlConversionsReader.GetColumnTypeDescription(ConversionXmlHelper.OracleBool());
            colDesc.TypeName.Should().Be("number", "because type name is number");
            colDesc.ConvertTo.Should().Be("bool", "because number(1,0) should be bool");
            colDesc.Constraints.Count.Should().Be(2, "because we should have Precision and Scale");

            colDesc.Constraints[0].ConstraintType.Should().Be(ConstraintTypeName.Prec);
            colDesc.Constraints[0].Operator.OperatorName.Should().Be(TypeOperatorName.Eq);
            colDesc.Constraints[0].Operator.ConstraintValues.Count.Should().Be(1, "because there is only one value for Precision constraint");
            colDesc.Constraints[0].Operator.ConstraintValues[0].Should().Be(1, "because Bool has a precision of 1");

            colDesc.Constraints[1].ConstraintType.Should().Be(ConstraintTypeName.Scale);
            colDesc.Constraints[1].Operator.OperatorName.Should().Be(TypeOperatorName.Eq);
            colDesc.Constraints[1].Operator.ConstraintValues.Count.Should().Be(1, "because there is only one value for Scale constraint");
            colDesc.Constraints[1].Operator.ConstraintValues[0].Should().Be(0, "because Bool has a Scale of 0");

        }
    }
}