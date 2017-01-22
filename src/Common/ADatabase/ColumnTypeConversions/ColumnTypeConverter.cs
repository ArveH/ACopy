using System;
using System.Collections.Generic;
using System.Xml;
using ADatabase.Exceptions;

namespace ADatabase
{
    public class ColumnTypeConverter : IColumnTypeConverter
    {
        private readonly IXmlConversionsReader _xmlConversionsReader;
        private List<ITypeDescription> _types = new List<ITypeDescription>();

        public ColumnTypeConverter(IXmlConversionsReader xmlConversionsReader)
        {
            _xmlConversionsReader = xmlConversionsReader;
        }

        public string FromSystem { get; private set; }
        public string ToSystem { get; private set; }

        public void Initialize(string conversionXml)
        {
            try
            {
                var rootNode = _xmlConversionsReader.GetRootNode(conversionXml);
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
    }
}