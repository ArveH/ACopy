namespace ADatabase.SqlServer.Columns
{
    public class SqlServerSmallMoneyColumn: SqlServerMoneyColumn
    {
        public SqlServerSmallMoneyColumn(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnTypeName.SmallMoney;
        }

        public override string TypeToString()
        {
            return "smallmoney";
        }
    }
}