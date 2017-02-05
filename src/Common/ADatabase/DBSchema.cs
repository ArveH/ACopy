using System;
using System.Collections.Generic;
using System.Threading;
using ADatabase.Interfaces;

namespace ADatabase
{
    public abstract class DbSchema: IDbSchema
    {
        protected IDbContext DbContext;

        protected DbSchema(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private ICommands _commands;
        protected ICommands Commands
        {
            get
            {
                return _commands ?? (_commands = DbContext.PowerPlant.CreateCommands());
            }
        }

        public abstract List<ITableShortInfo> GetTableNames(string searchString);
        public abstract ITableDefinition GetTableDefinition(IColumnTypeConverter columnTypeConverter, string name);
        public abstract bool IsTable(string tableName);
        public abstract void CreateTable(ITableDefinition tableDefinition);
        public abstract bool IsIndex(string indexName, string tableName);

        public void DropTable(string name, bool checkIfExists = true)
        {
            if (checkIfExists && IsTable(name) == false)
            {
                return;
            }

            Commands.ExecuteNonQuery(string.Format("drop table {0}", name));
        }

        protected abstract List<IIndexColumn> GetIndexColumnsForIndex(IIndexDefinition index);
        protected abstract List<IIndexDefinition> GetIndexesForTable(string tableName);

        public List<IIndexDefinition> GetIndexDefinitions(string tableName)
        {
            List<IIndexDefinition> indexes = GetIndexesForTable(tableName);
            foreach (var index in indexes)
            {
                index.Columns = GetIndexColumnsForIndex(index);
            }

            return indexes;
        }

        public void CreateIndexes(List<IIndexDefinition> indexes, bool useLocation = false)
        {
            if (indexes == null)
            {
                return;
            }
            foreach (var index in indexes)
            {
                CreateIndex(index, useLocation);
            }
        }

        public abstract string GetLocationAsSql(string location);

        public void CreateIndex(IIndexDefinition index, bool useLocation = false)
        {
            if (!IsIndexForThisDb(index))
            {
                return;
            }

            string createStmt = string.Format("create {2}{3}index {0} on {1} (", 
                index.IndexName, 
                index.TableName, 
                index.IsUnique?"unique ":"", 
                index.IsClustered?"clustered ":"");
            for (int i = 0; i < index.Columns.Count; i++)
            {
                if (i > 0)
                {
                    createStmt += ", ";
                }
                createStmt += index.Columns[i].IsExpression ? index.Columns[i].Expression : index.Columns[i].Name;
            }
            createStmt += ") ";
            if (useLocation)
            {
                createStmt += GetLocationAsSql(index.Location);
            }

            Commands.ExecuteNonQuery(createStmt);
        }

        private bool IsIndexForThisDb(IIndexDefinition index)
        {
            return index.DbSpecific == DbTypeName.Any || index.DbSpecific == DbContext.DbType;
        }

        public void DropView(string name, bool checkIfExists = true)
        {
            if (checkIfExists && IsView(name) == false)
            {
                return;
            }

            Commands.ExecuteNonQuery(string.Format("drop view {0}", name));
        }

        public abstract bool IsView(string viewName);

        public long GetRowCount(string tableName)
        {
            return Convert.ToInt64(Commands.ExecuteScalar(string.Format("select count(*) from {0}", tableName)));
        }

        public virtual string GetCollation()
        {
            throw new NotImplementedException();
        }

        public abstract bool CanConnect(CancellationToken token);
    }
}
