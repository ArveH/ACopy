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
        private readonly IXmlConversionsReader _xmlConversionsReader = new XmlConversionsReader();

        [TestMethod]
        public void TestInitialization_When_IllegalXml()
        {
            Action act = () => _xmlConversionsReader.GetRootNode("<illegal xml>");
            act.ShouldThrow<XmlException>().WithMessage("Error when reading conversion XML");
        }

        [TestMethod]
        public void TestInitialization_When_LegalXmlButIncorrectRootElement()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.LegalXmlButIncorrectRootElement());
            act.ShouldThrow<XmlException>().WithMessage("Can't find root element 'TypeConversions'");
        }

        [TestMethod]
        public void TestInitialization_When_FromAttributeMissing()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.FromAttributeMissing());
            act.ShouldThrow<XmlException>().WithMessage("Error with attribute 'From' for 'TypeConversions'");
        }

        [TestMethod]
        public void TestInitialization_When_ToAttributeMissing()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.ToAttributeBlank());
            act.ShouldThrow<XmlException>().WithMessage("Error with attribute 'To' for 'TypeConversions'");
        }

        [TestMethod]
        public void TestInitialization_When_LegalXmlButNoConversions()
        {
            Action act = () => _xmlConversionsReader.GetRootNode(ConversionXmlHelper.LegalXmlButNoConversions());
            act.ShouldThrow<XmlException>().WithMessage("No conversions found");
        }
    }
}