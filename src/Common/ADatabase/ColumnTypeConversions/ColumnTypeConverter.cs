using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ADatabase.Exceptions;
using ADatabase.Extensions;

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

        public string GetDestinationType(string sourceType, ref int length, ref int prec, ref int scale)
        {
            foreach (var type in _types)
            {
                if (type.Validate(sourceType, length, prec, scale))
                {

                    return type.GetDestinationType(ref length, ref prec, ref scale);
                }
            }

            throw new AColumnTypeException($"Illegal type: '{sourceType}', length={length}, prec={prec}, scale={scale}");
        }
    }
}