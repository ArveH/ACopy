namespace ADatabase.Oracle.Columns
{
    public class OracleCharColumn : OracleVarchar2Column
    {
        public OracleCharColumn(string name, int length, bool isNullable, string def)
            : base(name, length, isNullable, def)
        {
            Type = ColumnTypeName.Char;
        }

        public override string TypeToString()
        {
            return $"char({(int) Details["Length"]} char)";
        }
    }
}