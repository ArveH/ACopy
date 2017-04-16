using System;
using System.Collections.Generic;
using System.Linq;

namespace ADatabase
{
    public class TableDefinition : ITableDefinition
    {
        public TableDefinition(string name, List<IColumn> columns, string location)
        {
            Name = name;
            Location = location;
            Columns = columns;
            HasBlobColumn = false;
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public List<IColumn> Columns { get; private set; }

        public List<IIndexDefinition> Indexes { get; set; }

        public bool HasBlobColumn { get; set; }

        /// <summary>
        /// Special handling used because of a bug in Oracle. We temporarily set raw(16) columns to raw(17).
        /// This method will find all guid and raw(16) columns in the table definition.
        /// </summary>
        public List<string> GetRaw16Columns()
        {
            var raw16ColumnNames = new List<string>();
            var raw16Columns = Columns
                .FindAll(
                    c =>
                        (c.Type == ColumnTypeName.Guid || c.Type == ColumnTypeName.Raw) &&
                        c.Details.ContainsKey("Length") &&
                        (int) c.Details["Length"] == 16);
            if (raw16Columns.Count > 0)
            {
                raw16ColumnNames.AddRange(
                    raw16Columns.Select(c => c.Name));
            }

            return raw16ColumnNames;
        }

        public static DbTypeName ConvertStringToDbType(string dbType)
        {
            if (string.Compare(dbType, "oracle", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbTypeName.Oracle;
            }
            if (string.Compare(dbType, "sqlserver", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbTypeName.SqlServer;
            }
            return DbTypeName.Any;
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