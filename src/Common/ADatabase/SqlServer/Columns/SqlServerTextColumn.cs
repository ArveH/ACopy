using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerTextColumn: SqlServerVarcharColumn
    {
        public SqlServerTextColumn(string name, bool isNullable, string def, string collation)
            : base(name, -1, isNullable, AdjustDefaultValue(def), collation)
        {
            Type = ColumnTypeName.OldText;
        }

        public override string TypeToString()
        {
            return "text";
        }
    }
}