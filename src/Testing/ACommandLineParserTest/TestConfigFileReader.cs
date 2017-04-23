using System;
using System.Text;
using System.Xml;
using ACommandLineParser.ConfigFileReader;
using ADatabase;
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
            var xmlDoc = GetConnectionStringsXml(null);

            var connectionStrings = _configFileReader.GetConnectionStrings(xmlDoc);
            connectionStrings.Count.Should().Be(0, "because there are no connectionStrings in the config file");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_AddTagMissing()
        {
            var xmlDoc = GetConnectionStringsXml("    <illegal key=\"cc\"/>");

            Action act = () => _configFileReader.GetConnectionStrings(xmlDoc);
            act.ShouldThrow<XmlException>().WithMessage("Error with 'Add' tag in connectionStrings section");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_NameAttributeMissing()
        {
            var xmlDoc = GetConnectionStringsXml("    <add key=\"cc\"/>");

            Action act = () => _configFileReader.GetConnectionStrings(xmlDoc);
            act.ShouldThrow<XmlException>().WithMessage("Error with 'name' attribute in connectionStrings section");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_ConnectionStringAttributeMissing()
        {
            var xmlDoc = GetConnectionStringsXml("    <add name=\"cc\"/>");

            Action act = () => _configFileReader.GetConnectionStrings(xmlDoc);
            act.ShouldThrow<XmlException>().WithMessage("Error with 'connectionString' attribute in connectionStrings section");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_One()
        {
            var txt =
                "    <add name=\"aw\" connectionString=\"Trusted_Connection=True;database=AdventureWorks;server=(local)\"/>";
            var xmlDoc = GetConnectionStringsXml(txt);

            var connectionStrings = _configFileReader.GetConnectionStrings(xmlDoc);
            connectionStrings.Count.Should().Be(1, "because we got one connection string in acopy.xml");
            connectionStrings["aw"].Length.Should().BeGreaterThan(20, "because it's usually at least that long");
        }

        [TestMethod]
        public void TestGetConnectionStrings_When_Two()
        {
            var txt =
                "    <add name=\"aw\" connectionString=\"Trusted_Connection=True;database=AdventureWorks;server=(local)\"/>" +
                "    <add name=\"bw\" connectionString=\"Trusted_Connection=True;database=AdventureWorks;server=(local)\"/>";
            var xmlDoc = GetConnectionStringsXml(txt);

            var connectionStrings = _configFileReader.GetConnectionStrings(xmlDoc);
            connectionStrings.Count.Should().Be(2, "because we got two connection strings in acopy.xml");
        }

        [TestMethod]
        public void TestGetConversionFile_When_XmlInputIsNull()
        {
            Action act = () => _configFileReader.GetConversionFile(null, null, null);
            act.ShouldThrow<ArgumentNullException>().Where(m => m.Message.StartsWith("null value in GetConversionFile"));
        }

        [TestMethod]
        public void TestGetConversionFile_When_RdbmsIsNull()
        {
            Action act = () => _configFileReader.GetConversionFile(new XmlDocument(), null, null);
            act.ShouldThrow<ArgumentException>().Where(m => m.Message.StartsWith("illegal value ''"));
        }

        [TestMethod]
        public void TestGetConversionFile_When_DirectionIsIllegal()
        {
            Action act = () => _configFileReader.GetConversionFile(new XmlDocument(), DatabaseSystemName.Oracle, "test");
            act.ShouldThrow<ArgumentException>().Where(m => m.Message.StartsWith("illegal value 'test'"));
        }

        [TestMethod]
        public void TestConversionFile_When_SectionNotFound()
        {
            var xmlString =
                GetConfigurationXmlString();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            Action act = () => _configFileReader.GetConversionFile(
                xmlDoc, DatabaseSystemName.Oracle, CopyDirection.FromTableToFile);
            act.ShouldThrow<XmlException>().WithMessage("conversionFiles tag not found");
        }

        [TestMethod]
        public void TestConversionFile_When_ConversionFileNotFound()
        {
            var xmlDoc = GetConversionFilesXml(null);

            Action act = () => _configFileReader.GetConversionFile(
                xmlDoc, DatabaseSystemName.Oracle, CopyDirection.FromTableToFile);
            act.ShouldThrow<XmlException>().WithMessage("'oracleFromTableToFile' tag not found");
        }

        [TestMethod]
        public void TestConversionFile_When_OracleFromTableToFile()
        {
            var xmlDoc = GetConversionFilesXml("    <oracleFromTableToFile value=\"ConversionFiles\\OracleWriterConversions.xml\"/>");

            var path = _configFileReader.GetConversionFile(
                xmlDoc, DatabaseSystemName.Oracle, CopyDirection.FromTableToFile);
            path.Should().Be(@"ConversionFiles\OracleWriterConversions.xml");
        }

        [TestMethod]
        public void TestConversionFile_When_sqlServerFromFileToTable()
        {
            var txt =
                "    <oracleFromTableToFile value=\"ConversionFiles\\OracleWriterConversions.xml\"/>" +
                "    <sqlServerFromFileToTable value=\"ConversionFiles\\SqlServerReaderConversions.xml\"/>";
            var xmlDoc = GetConversionFilesXml(txt);

            var path = _configFileReader.GetConversionFile(
                xmlDoc, DatabaseSystemName.SqlServer, CopyDirection.FromFileToTable);
            path.Should().Be(@"ConversionFiles\SqlServerReaderConversions.xml");
        }

        #region XML Text creation

        private XmlDocument GetConversionFilesXml(string txt)
        {
            var xmlString =
                GetConfigurationXmlString(
                    null, null, null, 
                    GetConversionFilesXmlString(txt));
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }

        private XmlDocument GetConnectionStringsXml(string txt)
        {
            var xmlString =
                GetConfigurationXmlString(
                    GetConnectionStringsXmlString(txt));
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }

        private string GetConfigurationXmlString(
            string connectionStrings = null,
            string commandLineParameters = null,
            string configurationParameters = null,
            string conversionFiles = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<configuration>");
            if (connectionStrings != null) sb.AppendLine(connectionStrings);
            if (commandLineParameters != null) sb.AppendLine(commandLineParameters);
            if (configurationParameters != null) sb.AppendLine(configurationParameters);
            if (conversionFiles != null) sb.AppendLine(conversionFiles);
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

        private string GetConversionFilesXmlString(string conversionFiles)
        {
            var sb = new StringBuilder();
            sb.AppendLine("  <conversionFiles>");
            if (conversionFiles != null) sb.AppendLine(conversionFiles);
            sb.AppendLine("  </conversionFiles>");
            return sb.ToString();
        }

        #endregion
    }
}