namespace ADatabase.Oracle.Columns
{
    public class OracleClobColumn: OracleVarchar2Column
    {
        public OracleClobColumn(string name, bool isNullable, string def)
            : base(name, -1, isNullable, def)
        {
            Type = ColumnTypeName.LongText;
        }

        public override string TypeToString()
        {
            return "clob";
        }
    }
}
