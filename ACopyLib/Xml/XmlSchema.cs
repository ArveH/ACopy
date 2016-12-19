using System.Linq;
using ACopyLib.U4Indexes;
using ADatabase;

namespace ACopyLib.Xml
{
    public class XmlSchema: IXmlSchema
    {
        readonly IDbContext _dbContext;
        readonly IDbSchema _dbSchema;
        public XmlSchema(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSchema = _dbContext.PowerPlant.CreateDbSchema();
        }

        IAXmlWriter _xmlWriter;
        public IAXmlWriter XmlWriter
        {
            get { return _xmlWriter ?? (_xmlWriter = AXmlFactory.CreateWriter()); }
            set { _xmlWriter = value; }
        }

        IAXmlReader _xmlReader;
        private IAXmlReader XmlReader
        {
            get { return _xmlReader ?? (_xmlReader = AXmlFactory.CreateReader(_dbContext)); }
        }

        IU4Indexes _u4Indexes;
        public IU4Indexes U4Indexes
        {
            get { return _u4Indexes ?? (_u4Indexes = U4IndexesFactory.CreateInstance(_dbContext)); }
            set { _u4Indexes = value; }
        }

        public ITableDefinition Write(string directory, string tableName, string schemaFileSuffix)
        {
            ITableDefinition tableDefinition = _dbSchema.GetTableDefinition(tableName);

            tableDefinition.Indexes = _dbSchema.GetIndexDefinitions(tableName);
            var indexesNotAlreadyOnTable = U4Indexes.GetIndexes(tableName)
                .Where(i => tableDefinition.Indexes.All(i2 => i2.IndexName != i.IndexName));
            tableDefinition.Indexes.AddRange(indexesNotAlreadyOnTable);

            XmlWriter.WriteSchema(tableDefinition, directory + tableDefinition.Name + "." + schemaFileSuffix);
            return tableDefinition;
        }

        public ITableDefinition GetTableDefinition(string fileName)
        {
            return XmlReader.ReadSchema(fileName);
        }
    }
}
