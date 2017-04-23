using System;
using System.Text;
using System.Xml;
using ACommandLineParser.ConfigFileReader;
using ALogger;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ACommandLineParserTest
{
    [TestClass]
    public class TestConfigFileReader
    {
        [TestMethod]
        public void TestGetConnectionStrings_When_SectionNotFound()
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<configuration>");
            sb.Append("</configuration>");

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sb.ToString());

            var loggerMock = new Mock<IALogger>();
            var configFileReader = new ConfigFileReader(loggerMock.Object);
            Action act = () => configFileReader.GetConnectionStrings(xmlDoc);
            act.ShouldThrow<XmlException>().WithMessage("connectionStrings tag not found");
        }

    }
}