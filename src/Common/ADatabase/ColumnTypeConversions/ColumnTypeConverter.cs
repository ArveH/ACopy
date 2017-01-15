using System;
using System.Collections.Generic;
using System.Xml;
using ADatabase.Exceptions;

namespace ADatabase
{
    public class ColumnTypeConverter : IColumnTypeConverter
    {
        private List<IColumnTypeDescription> _types = new List<IColumnTypeDescription>();

        public ColumnTypeConverter()
        {
        }

        public string FromSystem { get; private set; }
        public string ToSystem { get; private set; }

        public void Initialize(string conversionXml)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(conversionXml);
                var rootNode = GetRootNode(xmlDocument);

                if (rootNode.ChildNodes.Count == 0) throw new AColumnTypeException("No conversions found");

                foreach (XmlNode typeNode in rootNode.ChildNodes)
                {
                    var typeName = typeNode.Attributes?["Name"].InnerText ?? "";
                    var toType = typeNode.Attributes?["To"].InnerText ?? "";
                }
            }
            catch (Exception ex)
            {
                if (ex is AColumnTypeException) throw;

                throw new AColumnTypeException("Error when reading conversion XML", ex);
            }
        }

        private XmlNode GetRootNode(XmlDocument xmlDocument)
        {
            var rootNode = xmlDocument.DocumentElement?.SelectSingleNode("/TypeConversions");
            if (rootNode == null) throw new AColumnTypeException("Can't find root element 'TypeConversions'");
            if (rootNode.Attributes == null) throw new AColumnTypeException("Root element 'TypeConversions' has no attributes");
            FromSystem = rootNode.Attributes["From"]?.InnerText;
            if (string.IsNullOrWhiteSpace(FromSystem))
                throw new AColumnTypeException("Error with attribute 'From' for 'TypeConversions'");
            ToSystem = rootNode.Attributes["To"]?.InnerText;
            if (string.IsNullOrWhiteSpace(ToSystem))
                throw new AColumnTypeException("Error with attribute 'To' for 'TypeConversions'");

            return rootNode;
        }
    }
}