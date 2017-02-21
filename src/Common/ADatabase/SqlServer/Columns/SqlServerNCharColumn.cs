namespace ADatabase.SqlServer.Columns
{
    public class SqlServerNCharColumn: SqlServerVarcharColumn
    {
        public SqlServerNCharColumn(string name, int length, bool isNullable, string def, string collation): 
            base(name, length, isNullable, def, collation)
        {
            Type = ColumnTypeName.NChar;
        }

        public override string TypeToString()
        {
            return $"nchar({(int) Details["Length"]})";
        }
    }
}