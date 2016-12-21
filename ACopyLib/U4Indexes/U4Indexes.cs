using System;
using System.Collections.Generic;
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

        public string AagTableName { get; set; } = "aagindex";
        public string AsysTableName { get; set; } = "asysindex";

        private List<IIndexDefinition> ReadIndexDefinitionsFromDatabase(string selectStatement, string tableName)
        {
            var indexDefinitions = new List<IIndexDefinition>();

            IDataCursor cursor = null;
            try
            {
                cursor = _dbContext.PowerPlant.CreateDataCursor();
                var reader = cursor.ExecuteReader(selectStatement);
                while (reader.Read())
                {
                    var indexName = reader.GetString(0);
                    var columnList = reader.GetString(1);
                    var location = reader.GetString(2);
                    var isUnique = Convert.ToBoolean(reader.GetValue(3));
                    var dbName = reader.GetString(4);
                    var indexDefinition = _dbContext.PowerPlant.CreateIndexDefinition(indexName, tableName, location, isUnique);
                    indexDefinition.Columns = SplitColumns(columnList);
                    indexDefinition.DbSpecific = TableDefinition.ConvertStringToDbType(dbName);
                    indexDefinitions.Add(indexDefinition);

                }
            }
            finally
            {
                cursor?.Close();
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

        private static bool IsLegalName(string column)
        {
            return column.All(c => char.IsLetterOrDigit(c) || c=='_');
        }

        private List<IIndexDefinition> GetIndexesFromIndexTable(string indexesTable, string tableName)
        {
            var selectStatement = "";
            selectStatement += "SELECT index_name, column_list, location_name, unique_flag, db_name ";
            selectStatement += $"  FROM {indexesTable} ";
            selectStatement += $" WHERE table_name = '{tableName.ToLower()}' ";

            return ReadIndexDefinitionsFromDatabase(selectStatement, tableName);
        }

        public List<IIndexDefinition> GetIndexes(string tableName)
        {
            var indexes = GetIndexesFromIndexTable(AagTableName, tableName);
            var asysIndexes = GetIndexesFromIndexTable(AsysTableName, tableName);
            indexes.AddRange(
                asysIndexes.Where(i => indexes.All(i2 => i2.IndexName != i.IndexName))
                );

            return indexes;
        }
    }
}
