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
            CheckRdbms(rootNode);
            CheckDirection(rootNode);

            if (rootNode.ChildNodes.Count == 0) throw new XmlException("No conversions found");

            return rootNode;
        }

        public ITypeDescription GetColumnTypeDescription(XmlNode xmlNode)
        {
            var colDesc = _typeDescriptionFactory.GetColumnTypeDescription();
            GetTypeAttributes(xmlNode, colDesc);

            if (!xmlNode.HasChildNodes) return colDesc;

            CheckTypeDetails(xmlNode);
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                GetTypeConstraints(childNode, colDesc);
            }

            return colDesc;
        }

        public string GetRdbms(XmlNode rootNode)
        {
            return rootNode.Attributes?["Database"]?.InnerText;
        }

        public string GetDirection(XmlNode rootNode)
        {
            return rootNode.Attributes?["Direction"]?.InnerText;
        }

        #region Private

        private static void CheckDirection(XmlNode rootNode)
        {
            var direction = rootNode.Attributes?["Direction"]?.InnerText;
            if (string.IsNullOrWhiteSpace(direction))
                throw new XmlException("Error with attribute 'Direction' for 'TypeConversions'");
            if (direction != CopyDirection.FromFileToTable && direction != CopyDirection.FromTableToFile)
                throw new XmlException(
                    $"Illegal value '{direction}' for attribute 'Direction'. Legal values are '{CopyDirection.FromFileToTable}' or '{CopyDirection.FromTableToFile}'.");
        }

        private static void CheckRdbms(XmlNode rootNode)
        {
            var rdbms = rootNode.Attributes?["Database"]?.InnerText;
            if (string.IsNullOrWhiteSpace(rdbms))
                throw new XmlException("Error with attribute 'Database' for 'TypeConversions'");
        }

        private static bool IsLegalTypeDetail(string detail)
        {
            return detail == "Prec" || detail == "Scale" || detail == "Length";
        }

        private void CheckTypeDetails(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!IsLegalTypeDetail(childNode.Name)) throw new XmlException($"Illegal type detail '{childNode.Name}' for type '{xmlNode.Attributes?["Source"].InnerText}'");
            }
        }

        private static void GetTypeAttributes(XmlNode xmlNode, ITypeDescription colDesc)
        {
            var sourceType = xmlNode.Attributes?["Source"]?.InnerText;
            if (string.IsNullOrWhiteSpace(sourceType))
                throw new XmlException("Error with attribute 'Name' for 'Type'");
            var destinationType = xmlNode.Attributes?["Destination"]?.InnerText;
            if (string.IsNullOrWhiteSpace(destinationType))
                throw new XmlException("Error with attribute 'To' for 'Type'");

            colDesc.TypeName = sourceType;
            colDesc.ConvertTo = destinationType;
        }

        private static void GetTypeConstraints(XmlNode xmlNode, ITypeDescription colDesc)
        {
            var constraintName = xmlNode.Name;
            var opName = xmlNode.Attributes?["Operator"].InnerText;
            if (opName == "in")
            {
                var constraintValues = xmlNode.InnerText.Split(',').Select(v => Convert.ToInt32(v));
                colDesc.AddConstraint(constraintName, opName, constraintValues);
            }
            else
            {
                var constraintValue = Convert.ToInt32(xmlNode.InnerText);
                colDesc.AddConstraint(constraintName, opName, constraintValue);
            }
        }

        #endregion
    }
}