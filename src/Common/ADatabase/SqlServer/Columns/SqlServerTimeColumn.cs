namespace ADatabase.SqlServer.Columns
{
    public class SqlServerTimeColumn: SqlServerDatetimeColumn
    {
        private readonly string _typeToString;

        public SqlServerTimeColumn(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnTypeName.Time;
        }

        public override string TypeToString()
        {
            return "time";
        }

    }
}