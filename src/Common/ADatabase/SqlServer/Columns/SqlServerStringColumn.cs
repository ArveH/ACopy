namespace ADatabase.SqlServer.Columns
{
    public class SqlServerStringColumn: SqlServerVarcharColumn
    {
        public SqlServerStringColumn(string name, int length, bool isNullable, string def, string collation)
            : base(name, length, isNullable, def, collation)
        {
            Type = ColumnTypeName.String;
        }

        public override string TypeToString()
        {
            return string.Format("nvarchar({0})", (int)Details["Length"]);
        }
    }
}
