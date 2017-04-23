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
            var csNode = xmlDoc.DocumentElement?.SelectSingleNode("connectionStrings");
            if (csNode == null) throw new XmlException("connectionStrings tag not found");

            foreach (XmlNode node in csNode.ChildNodes)
            {
                AddConnectionString(connectionStrings, node);
            }

            return connectionStrings;
        }

        public string GetConversionFile(XmlDocument xmlDoc, string rdbms, string direction)
        {
            throw new System.NotImplementedException();
        }

        private void AddConnectionString(Dictionary<string, string> connectionStrings, XmlNode node)
        {
            if (node.Name != "add") throw new XmlException("Error with 'Add' tag in connectionStrings section");
            var name = node.Attributes?["name"]?.InnerText;
            if (name == null) throw new XmlException("Error with 'name' attribute in connectionStrings section");
            var cs = node.Attributes?["connectionString"]?.InnerText;
            if (cs == null) throw new XmlException("Error with 'connectionString' attribute in connectionStrings section");

            connectionStrings.Add(name, cs);
        }
    }
}