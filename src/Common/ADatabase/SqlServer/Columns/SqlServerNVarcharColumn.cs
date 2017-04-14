namespace ADatabase.SqlServer.Columns
{
    public class SqlServerNVarcharColumn: SqlServerVarcharColumn
    {
        public SqlServerNVarcharColumn(string name, int length, bool isNullable, string def, string collation)
            : base(name, length, isNullable, def, collation)
        {
            if (length == -1)
            {
                Type = ColumnTypeName.NLongText;
                TypeString = "nvarchar(max)";
                Details.Remove("Length");
            }
            else
            {
                Type = ColumnTypeName.NVarchar;
                TypeString = $"nvarchar({length})";
            }
        }
    }
}
