using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ADatabase;
using ADatabase.Interfaces;

namespace ACopyLib.U4Indexes
{
    public class U4Indexes: IU4Indexes
    {
        private readonly IDbContext _dbContext;

        public U4Indexes(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        string _aagTableName = "aagindex";
        public string AagTableName
        {
            get { return _aagTableName; }
            set { _aagTableName = value; }
        }

        string _asysTableName = "asysindex";
        public string AsysTableName
        {
            get { return _asysTableName; }
            set { _asysTableName = value; }
        }

        private List<IIndexDefinition> ReadIndexDefinitionsFromDatabase(string selectStatement, string tableName)
        {
            List<IIndexDefinition> indexDefinitions = new List<IIndexDefinition>();

            IDataCursor cursor = null;
            try
            {
                cursor = _dbContext.PowerPlant.CreateDataCursor();
                IDataReader reader = cursor.ExecuteReader(selectStatement);
                while (reader.Read())
                {
                    string indexName = reader.GetString(0);
                    string columnList = reader.GetString(1);
                    string location = reader.GetString(2);
                    bool isUnique = Convert.ToBoolean(reader.GetValue(3));
                    string dbName = reader.GetString(4);
                    var indexDefinition = _dbContext.PowerPlant.CreateIndexDefinition(indexName, tableName, location, isUnique);
                    indexDefinition.Columns = SplitColumns(columnList);
                    indexDefinition.DbSpecific = TableDefinition.ConvertStringToDbType(dbName);
                    indexDefinitions.Add(indexDefinition);

                }
            }
            finally
            {
                if (cursor != null) cursor.Close();
            }

            return indexDefinitions;
        }

        private List<IIndexColumn> SplitColumns(string columnList)
        {
            return columnList
                .Split(
                    new[] { ',', ' ' }, 
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(
                    column => IsLegalName(column) 
                        ? IndexColumnFactory.CreateInstance(column) 
                        : IndexColumnFactory.CreateInstance("expression", column)).ToList();
        }

        private bool IsLegalName(string column)
        {
            return column.All(c => Char.IsLetterOrDigit(c) || c=='_');
        }

        private List<IIndexDefinition> GetIndexesFromIndexTable(string indexesTable, string tableName)
        {
            string selectStatement = "";
            selectStatement += "SELECT index_name, column_list, location_name, unique_flag, db_name ";
            selectStatement += string.Format("  FROM {0} ", indexesTable);
            selectStatement += string.Format(" WHERE table_name = '{0}' ", tableName.ToLower());

            return ReadIndexDefinitionsFromDatabase(selectStatement, tableName);
        }

        public List<IIndexDefinition> GetIndexes(string tableName)
        {
            List<IIndexDefinition> indexes = GetIndexesFromIndexTable(AagTableName, tableName);
            List<IIndexDefinition> asysIndexes = GetIndexesFromIndexTable(AsysTableName, tableName);
            indexes.AddRange(
                asysIndexes.Where(i => indexes.All(i2 => i2.IndexName != i.IndexName))
                );

            return indexes;
        }
    }
}
