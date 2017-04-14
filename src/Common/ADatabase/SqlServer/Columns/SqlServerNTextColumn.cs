namespace ADatabase.SqlServer.Columns
{
    public class SqlServerNTextColumn : SqlServerVarcharColumn
    {
        public SqlServerNTextColumn(string name, bool isNullable, string def, string collation)
            : base(name, -1, isNullable, AdjustDefaultValue(def), collation)
        {
            Type = ColumnTypeName.NOldText;
        }

        public override string TypeToString()
        {
            return "ntext";
        }
    }
}