namespace ADatabase.Oracle.Columns
{
    public class OracleNVarchar2Column: OracleVarchar2Column
    {
        public OracleNVarchar2Column(string name, int length, bool isNullable, string def)
            : base(name, length, isNullable, def)
        {
            Type = ColumnTypeName.NChar;
        }

        public override string TypeToString()
        {
            return $"nvarchar2({(int)Details["Length"]})";
        }

    }
}