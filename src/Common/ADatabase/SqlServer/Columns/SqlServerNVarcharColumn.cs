namespace ADatabase.SqlServer.Columns
{
    public class SqlServerNVarcharColumn: SqlServerVarcharColumn
    {
        public SqlServerNVarcharColumn(string name, int length, bool isNullable, string def, string collation)
            : base(name, length, isNullable, def, collation)
        {
            Type = ColumnTypeName.NVarchar;
            TypeString = length == -1 ? "nvarchar(max)" : $"nvarchar({length})";
        }
    }
}
