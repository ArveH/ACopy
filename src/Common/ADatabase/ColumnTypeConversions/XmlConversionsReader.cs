using System;
using System.Xml;

namespace ADatabase
{
    public class XmlConversionsReader : IXmlConversionsReader
    {
        private readonly IColumnTypeDescriptionFactory _columnTypeDescriptionFactory;

        public XmlConversionsReader(IColumnTypeDescriptionFactory columnTypeDescriptionFactory)
        {
            _columnTypeDescriptionFactory = columnTypeDescriptionFactory;
        }

        public XmlNode GetRootNode(string xmlText)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode rootNode;
            try
            {
                xmlDocument.LoadXml(xmlText);
                rootNode = xmlDocument.DocumentElement?.SelectSingleNode("/TypeConversions");
            }
            catch (Exception ex)
            {
                throw new XmlException("Error when reading conversion XML", ex);
            }
            if (rootNode == null) throw new XmlException("Can't find root element 'TypeConversions'");
            if (rootNode.Attributes == null) throw new XmlException("Root element 'TypeConversions' has no attributes");
            var sourceSystem = rootNode.Attributes["From"]?.InnerText;
            if (string.IsNullOrWhiteSpace(sourceSystem))
                throw new XmlException("Error with attribute 'From' for 'TypeConversions'");
            var destinationSystem = rootNode.Attributes["To"]?.InnerText;
            if (string.IsNullOrWhiteSpace(destinationSystem))
                throw new XmlException("Error with attribute 'To' for 'TypeConversions'");

            if (rootNode.ChildNodes.Count == 0) throw new XmlException("No conversions found");

            return rootNode;
        }

        public IColumnTypeDescription GetColumnTypeDescription(XmlNode xmlNode)
        {
            var fromType = xmlNode.Attributes?["Name"]?.InnerText;
            if (string.IsNullOrWhiteSpace(fromType))
                throw new XmlException("Error with attribute 'Name' for 'Type'");
            var toType = xmlNode.Attributes?["To"]?.InnerText;
            if (string.IsNullOrWhiteSpace(toType))
                throw new XmlException("Error with attribute 'To' for 'Type'");

            var colDesc = _columnTypeDescriptionFactory.GetColumnTypeDescription();
            colDesc.TypeName = fromType;
            colDesc.ConvertTo = toType;

            return colDesc;
        }

        public string GetSourceSystem(XmlNode rootNode)
        {
            return rootNode.Attributes?["From"]?.InnerText;
        }

        public string GetDestinationSystem(XmlNode rootNode)
        {
            return rootNode.Attributes?["To"]?.InnerText;
        }
    }
}