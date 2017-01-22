using System.Collections.Generic;
using ADatabase.Interfaces;

namespace ADatabase
{
    public class IndexDefinition: IIndexDefinition
    {
        public IndexDefinition(string indexName, string tableName, string location, bool isUnique, int indexId=0, bool isClustered=false)
        {
            IndexName = indexName.ToLower();
            TableName = tableName;
            IndexId = indexId;
            Location = location.ToLower();
            IsUnique = isUnique;
            IsClustered = isClustered;
            Columns = null;
        }

        public string IndexName { get; private set; }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value.ToLower(); }
        }

        public int IndexId { get; private set; }

        public string Location { get; private set; }

        public bool IsUnique { get; private set; }

        public bool IsClustered { get; private set; }

        private DbTypeName _dbSpecific = DbTypeName.Any;
        public DbTypeName DbSpecific
        {
            get 
            {
                if (IsClustered)
                {
                    return DbTypeName.SqlServer;
                }
                return _dbSpecific; 
            }
            set { _dbSpecific = value; }
        }

        public List<IIndexColumn> Columns { get; set; }
    }
}
