namespace ADatabase.SqlServer.Columns
{
    public class SqlServerCharColumn: SqlServerVarcharColumn
    {
        public SqlServerCharColumn(string name, int length, bool isNullable, string def, string collation): base(name, length, isNullable, def, collation)
        {
            Type = ColumnTypeName.Char;
        }

        public override string TypeToString()
        {
            return string.Format("char({0})", (int)Details["Length"]);
        }
    }
}
