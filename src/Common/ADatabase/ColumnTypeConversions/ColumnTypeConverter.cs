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
                FromSystem = _xmlConversionsReader.GetSourceSystem(rootNode);
                ToSystem = _xmlConversionsReader.GetDestinationSystem(rootNode);
                foreach (XmlNode typeNode in rootNode.ChildNodes)
                {
                    var typeDescription = _xmlConversionsReader.GetColumnTypeDescription(typeNode);
                    _types.Add(typeDescription);
                }
            }
            catch (Exception ex)
            {
                if (ex is AColumnTypeException) throw;

                throw new AColumnTypeException("Error when reading conversion XML", ex);
            }
        }

        public string GetDestinationType(string nativeType, int? length, int? prec, int? scale)
        {
            string destinationType;

            foreach (var type in _types)
            {
                if (type.TypeName != nativeType) continue;

                foreach (var constraint in type.Constraints)
                {
                }
            }

            throw new AColumnTypeException($"Illegal type name '{nativeType}', length={length}, prec={prec}, scale={scale}");
        }
    }
}