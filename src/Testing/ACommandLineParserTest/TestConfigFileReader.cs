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
        Mock<IALogger> _loggerMock;
        IConfigFileReader _configFileReader;

        [TestInitialize]
        public void Startup()
        {
            _loggerMock = new Mock<IALogger>();
            _configFileReader = new ConfigFileReader(_loggerMock.Object);
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_InputIsNull()
        {
            Action act = () => _configFileReader.GetConnectionStrings(null);
            act.ShouldThrow<ArgumentNullException>().Where(c => c.Message.StartsWith("null value in GetConnectionStrings"));
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_SectionNotFound()
        {
            var xmlString = GetConfigurationXmlString(null);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            Action act = () => _configFileReader.GetConnectionStrings(xmlDoc);
            act.ShouldThrow<XmlException>().WithMessage("connectionStrings tag not found");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_SectionEmpty()
        {
            var xmlString = 
                GetConfigurationXmlString(
                    GetConnectionStringsXmlString(null));
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var connectionStrings = _configFileReader.GetConnectionStrings(xmlDoc);
            connectionStrings.Count.Should().Be(0, "because there are no connectionStrings in the config file");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_IllegalNode()
        {
            var xmlString =
                GetConfigurationXmlString(
                    GetConnectionStringsXmlString("    <illegal key=\"cc\"/>"));
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            Action act = () => _configFileReader.GetConnectionStrings(xmlDoc);
            act.ShouldThrow<XmlException>().WithMessage("Error in connectionStrings section");
        }

        #region XML Text creation

        private string GetConfigurationXmlString(string connectionStrings)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<configuration>");
            if (connectionStrings != null) sb.AppendLine(connectionStrings);
            sb.AppendLine("</configuration>");
            return sb.ToString();
        }

        private string GetConnectionStringsXmlString(string connectionStrings)
        {
            var sb = new StringBuilder();
            sb.AppendLine("  <connectionStrings>");
            if (connectionStrings != null) sb.AppendLine(connectionStrings);
            sb.AppendLine("  </connectionStrings>");
            return sb.ToString();
        }
        #endregion
    }
}