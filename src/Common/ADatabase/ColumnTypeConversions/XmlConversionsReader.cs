using System;
using System.Linq;
using System.Xml;

namespace ADatabase
{
    public class XmlConversionsReader : IXmlConversionsReader
    {
        private readonly ITypeDescriptionFactory _typeDescriptionFactory;

        public XmlConversionsReader(ITypeDescriptionFactory typeDescriptionFactory)
        {
            _typeDescriptionFactory = typeDescriptionFactory;
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

        public ITypeDescription GetColumnTypeDescription(XmlNode xmlNode)
        {
            var fromType = xmlNode.Attributes?["Name"]?.InnerText;
            if (string.IsNullOrWhiteSpace(fromType))
                throw new XmlException("Error with attribute 'Name' for 'Type'");
            var toType = xmlNode.Attributes?["To"]?.InnerText;
            if (string.IsNullOrWhiteSpace(toType))
                throw new XmlException("Error with attribute 'To' for 'Type'");

            var colDesc = _typeDescriptionFactory.GetColumnTypeDescription();
            colDesc.TypeName = fromType;
            colDesc.ConvertTo = toType;

            if (!xmlNode.HasChildNodes) return colDesc;
            CheckTypeDetails(xmlNode);

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                var constraintName = childNode.Name;
                var opName = childNode.Attributes?["Operator"].InnerText;
                if (opName == "in")
                {
                    var constraintValues = childNode.InnerText.Split(',').Select(v => Convert.ToInt32(v));
                    colDesc.AddConstraint(constraintName, opName, constraintValues);
                }
                else
                {
                    var constraintValue = Convert.ToInt32(childNode.InnerText);
                    colDesc.AddConstraint(constraintName, opName, constraintValue);
                }
            }

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

        #region Private

        private static bool IsLegalTypeDetail(string detail)
        {
            return detail == "Precision" || detail == "Scale" || detail == "Length";
        }

        private void CheckTypeDetails(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!IsLegalTypeDetail(childNode.Name)) throw new XmlException($"Illegal type detail '{childNode.Name}' for type '{xmlNode.Attributes?["Name"].InnerText}'");
            }
        }
        #endregion
    }
}