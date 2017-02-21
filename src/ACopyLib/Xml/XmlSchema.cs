using System.IO;
using System.Linq;
using System.Xml;
using ACopyLib.Exceptions;
using ACopyLib.U4Indexes;
using ADatabase;

namespace ACopyLib.Xml
{
    public class XmlSchema: IXmlSchema
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSchema _dbSchema;

        public XmlSchema(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSchema = _dbContext.PowerPlant.CreateDbSchema();
        }

        private IAXmlWriter _xmlWriter;
        public IAXmlWriter XmlWriter
        {
            get { return _xmlWriter ?? (_xmlWriter = AXmlFactory.CreateWriter()); }
            set { _xmlWriter = value; }
        }

        private IAXmlReader _xmlReader;
        private IAXmlReader XmlReader => _xmlReader ?? (_xmlReader = AXmlFactory.CreateReader(_dbContext));

        public IU4Indexes U4Indexes { get; set; }

        public ITableDefinition Write(string directory, IColumnTypeConverter columnsTypeConverter, string tableName, string schemaFileSuffix)
        {
            ITableDefinition tableDefinition = _dbSchema.GetTableDefinition(columnsTypeConverter, tableName);

            tableDefinition.Indexes = _dbSchema.GetIndexDefinitions(tableName);
            if (U4Indexes != null)
            {
                var indexesNotAlreadyOnTable = U4Indexes.GetIndexes(tableName)
                    .Where(i => tableDefinition.Indexes.All(i2 => i2.IndexName != i.IndexName));
                tableDefinition.Indexes.AddRange(indexesNotAlreadyOnTable);
            }

            XmlWriter.WriteSchema(tableDefinition, directory + tableDefinition.Name + "." + schemaFileSuffix);
            return tableDefinition;
        }

        public ITableDefinition GetTableDefinition(IColumnTypeConverter columnsTypeConverter, string fileName)
        {
            try
            {
                return XmlReader.ReadSchema(columnsTypeConverter, File.ReadAllText(fileName));
            }
            catch (XmlException ex)
            {
                throw new NotValidXmlException($"The schema file '{fileName}' is invalid", ex);
            }
        }
    }
}
