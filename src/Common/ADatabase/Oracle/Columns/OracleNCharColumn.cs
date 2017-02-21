namespace ADatabase.Oracle.Columns
{
    public class OracleNCharColumn: OracleVarchar2Column
    {
        public OracleNCharColumn(string name, int length, bool isNullable, string def)
            : base(name, length, isNullable, def)
        {
            Type = ColumnTypeName.NChar;
        }

        public override string TypeToString()
        {
            return $"nchar({(int)Details["Length"]})";
        }
    }
}