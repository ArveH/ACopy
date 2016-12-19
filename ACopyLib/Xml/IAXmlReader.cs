using ADatabase;

namespace ACopyLib.Xml
{
    public interface IAXmlReader
    {
        ITableDefinition ReadSchema(string fileName);
    }
}