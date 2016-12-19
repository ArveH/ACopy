using System.Collections.Generic;

namespace ADatabase
{
    public abstract class PowerPlant: IPowerPlant
    {
        public IDbContext DbContext { get; set; }

        public ITableDefinition CreateTableDefinition(string name, List<IColumn> columns, string location)
        {
            return new TableDefinition(name, columns, location);
        }
        public ITableDefinition CreateTableDefinition()
        {
            return new TableDefinition("", new List<IColumn>(), "");
        }

        public IIndexDefinition CreateIndexDefinition(string indexName, string tableName, string location, bool isUnique, int id = 0, bool isClustered = false)
        {
            return new IndexDefinition(indexName, tableName, location, isUnique, id, isClustered);
        }

        public abstract IDbSchema CreateDbSchema();
        public abstract ICommands CreateCommands();
        public abstract IColumnFactory CreateColumnFactory();
        public abstract IFastCopy CreateFastCopy();
        public abstract IDataCursor CreateDataCursor();
    }
}