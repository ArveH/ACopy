using System;
using System.Collections.Generic;
using System.Xml;
using ALogger;

namespace ACommandLineParser.ConfigFileReader
{
    public class ConfigFileReader : IConfigFileReader
    {
        private readonly IALogger _logger;

        public ConfigFileReader(IALogger logger)
        {
            _logger = logger;
        }

        public List<IArgument> GetArguments(XmlDocument xmlDoc)
        {
            var args = new List<IArgument>();

            return args;
        }

        public Dictionary<string, string> GetConnectionStrings(XmlDocument xmlDoc)
        {
            if (xmlDoc == null) throw new ArgumentNullException(nameof(xmlDoc), "null value in GetConnectionStrings");

            var connectionStrings = new Dictionary<string, string>();
            var rootNode = xmlDoc.DocumentElement?.SelectSingleNode("connectionStrings");
            if (rootNode == null) throw new XmlException("connectionStrings tag not found");

            return connectionStrings;
        }

        public string GetConversionFile(XmlDocument xmlDoc, string rdbms, string direction)
        {
            throw new System.NotImplementedException();
        }
    }
}