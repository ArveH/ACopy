using ADatabase;

namespace ACopyLib.Xml
{
    public interface IAXmlReader
    {
        ITableDefinition ReadSchema(IColumnTypeConverter columnsTypeConverter, string fileName);
    }
}