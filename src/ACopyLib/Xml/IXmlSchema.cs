using ACopyLib.U4Indexes;
using ADatabase;

namespace ACopyLib.Xml
{
    public interface IXmlSchema
    {
        IAXmlWriter XmlWriter { get; set; }
        IU4Indexes U4Indexes { get; set; }
        ITableDefinition Write(string directory, IColumnTypeConverter columnsTypeConverter, string tableName, string schemaFileSuffix);
        ITableDefinition GetTableDefinition(IColumnTypeConverter columnsTypeConverter, string fileName);
    }
}
