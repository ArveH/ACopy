using System;
using System.Collections.Generic;
using System.Xml;
using ADatabase;
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
            if (xmlDoc == null) throw new ArgumentNullException(nameof(xmlDoc), "null value in GetConversionFile");
            if (rdbms == null || (rdbms != DatabaseSystemName.Oracle && rdbms != DatabaseSystemName.SqlServer))
                throw new ArgumentException($"illegal value '{rdbms}'", nameof(rdbms));
            if (direction == null || (direction != CopyDirection.FromFileToTable && direction != CopyDirection.FromTableToFile))
                throw new ArgumentException($"illegal value '{direction}'", nameof(direction));

            var cfNode = xmlDoc.DocumentElement?.SelectSingleNode("conversionFiles");
            if (cfNode == null) throw new XmlException("conversionFiles tag not found");

            var tagName = $"{char.ToLower(rdbms[0])}{rdbms.Substring(1)}{direction}";
            var pathNode = xmlDoc.DocumentElement?.SelectSingleNode("conversionFiles/" + tagName);
            if (pathNode == null) throw new XmlException($"'{tagName}' tag not found");

            var path = pathNode.Attributes?["value"]?.InnerText;
            if (path == null) throw new XmlException($"Error with 'value' attribute for {tagName}");

            return path;
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