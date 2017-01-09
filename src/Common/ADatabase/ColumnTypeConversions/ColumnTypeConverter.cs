using System;
using System.Xml;
using ADatabase.Exceptions;

namespace ADatabase
{
    public class ColumnTypeConverter : IColumnTypeConverter
    {
        public void Initialize(string conversionXml)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(conversionXml);
                var rootNode = xmlDocument.DocumentElement?.SelectSingleNode("/TypeConversions");
                if (rootNode == null) throw new AColumnTypeException("Can't find root element 'TypeConversions'");
                if (rootNode.Attributes == null) throw new AColumnTypeException("Root element 'TypeConversions' has no attributes");
                var from = rootNode.Attributes["From"]?.InnerText;
                if (string.IsNullOrWhiteSpace(from)) throw new AColumnTypeException("Error with attribute 'From' for 'TypeConversions'");
                var to = rootNode.Attributes["To"]?.InnerText;
                if (string.IsNullOrWhiteSpace(to)) throw new AColumnTypeException("Error with attribute 'To' for 'TypeConversions'");

                if (rootNode.ChildNodes.Count == 0) throw new AColumnTypeException("No conversions found");
            }
            catch (Exception ex)
            {
                if (ex is AColumnTypeException) throw;

                throw new AColumnTypeException("Error when reading conversion XML", ex);
            }
        }
    }
}