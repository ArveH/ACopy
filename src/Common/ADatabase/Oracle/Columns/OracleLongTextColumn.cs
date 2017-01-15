namespace ADatabase.Oracle.Columns
{
    public class OracleLongTextColumn: OracleVarchar2Column
    {
        public OracleLongTextColumn(string name, bool isNullable, string def, string collation)
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
