using System.Collections.Generic;
using System.Threading;
using ADatabase.Interfaces;

namespace ADatabase
{
    public interface IDbSchema
    {
        List<ITableShortInfo> GetTableNames(string searchString);
        ITableDefinition GetTableDefinition(string name);
        bool IsTable(string tableName);
        long GetRowCount(string tableName);
        void CreateTable(ITableDefinition tableDefinition);
        void DropTable(string name, bool checkIfExists = true);
        bool IsIndex(string indexName, string tableName);
        List<IIndexDefinition> GetIndexDefinitions(string tableName);
        void CreateIndexes(List<IIndexDefinition> indexes, bool useLocation=false);
        void CreateIndex(IIndexDefinition index, bool useLocation = false);
        string GetLocationAsSql(string location);
        void DropView(string viewName, bool checkIfExists = true);
        bool IsView(string viewName);
        string GetCollation();
        bool CanConnect(CancellationToken token);
    }
}