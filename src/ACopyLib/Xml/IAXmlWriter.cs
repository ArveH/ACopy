using ADatabase;

namespace ACopyLib.Xml
{
    public interface IAXmlWriter
    {
        void WriteSchema(ITableDefinition tableDefinition, string fullPath);
    }
}