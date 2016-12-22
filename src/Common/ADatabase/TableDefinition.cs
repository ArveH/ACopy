using System;
using System.Collections.Generic;

namespace ADatabase
{
    public class TableDefinition : ITableDefinition
    {
        public TableDefinition(string name, List<IColumn> columns, string location)
        {
            Name = name;
            Location = location;
            Columns = columns;
            HasRawColumn = false;
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public List<IColumn> Columns { get; private set; }

        public List<IIndexDefinition> Indexes { get; set; }

        public bool HasRawColumn { get; set; }

        /// <summary>
        /// Special handling used because of a bug in Oracle. We temporarily set guid columns to raw(17) (instead of raw(16)).
        /// This method will look up all guid columns in the table definition, and set the length accordingly
        /// </summary>
        /// <param name="rawLength">Length for the guid column</param>
        public void SetSizeForGuid(int rawLength)
        {
            Columns
                .FindAll(c => c.Type == ColumnType.Guid)
                .ForEach(c => c.Details["length"] = rawLength);
        }

        public static DbType ConvertStringToDbType(string dbType)
        {
            if (String.Compare(dbType, "oracle", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbType.Oracle;
            }
            if (String.Compare(dbType, "sqlserver", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbType.SqlServer;
            }
            return DbType.Any;
        }

        public void SetCollation(string collation)
        {
            foreach (var column in Columns)
            {
                if (column.Details.ContainsKey("Collation"))
                {
                    column.Details["Collation"] = collation;
                }
            }
        }
    }
}