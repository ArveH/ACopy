namespace ADatabase.SqlServer.Columns
{
    public class SqlServerLongTextColumn: SqlServerVarcharColumn
    {
        public SqlServerLongTextColumn(string name, bool isNullable, string def, string collation)
            : base(name, -1, isNullable, def, collation)
        {
            Type = ColumnTypeName.LongText;
            TypeString = "nvarchar(max)";
        }
    }
}
