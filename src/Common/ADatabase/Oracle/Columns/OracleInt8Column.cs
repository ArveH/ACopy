namespace ADatabase.Oracle.Columns
{
    public class OracleInt8Column : OracleNumberColumn
    {
        public OracleInt8Column(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Int8, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "number(3)";
        }
    }
}
