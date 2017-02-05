using System.Collections.Generic;

namespace ADatabase
{
    public interface IPowerPlant
    {
        IDbContext DbContext { get; set; }
        ITableDefinition CreateTableDefinition(string name, List<IColumn> columns, string location);
        ITableDefinition CreateTableDefinition();
        IIndexDefinition CreateIndexDefinition(string indexName, string tableName, string location, bool isUnique, int id = 0, bool isClustered = false);
        IDbSchema CreateDbSchema();
        ICommands CreateCommands();
        IColumnFactory CreateColumnFactory();
        IFastCopy CreateFastCopy();
        IDataCursor CreateDataCursor();
        IColumnTypeConverter CreateColumnTypeConverter(string conversionsFile);
    }
}