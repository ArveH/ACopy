namespace ADatabase.Oracle.Columns
{
    public class OracleLongColumn : OracleVarchar2Column
    {
        public OracleLongColumn(string name, bool isNullable, string def)
            : base(name, -1, isNullable, def)
        {
            Type = ColumnTypeName.OldText;
            Details.Remove("Length");
        }

        public override string TypeToString()
        {
            return "long";
        }
    }
}