using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleNClobColumn: OracleVarchar2Column
    {
        public OracleNClobColumn(string name, bool isNullable, string def)
            : base(name, -1, isNullable, def)
        {
            Type = ColumnTypeName.NLongText;
        }

        public override string TypeToString()
        {
            return "nclob";
        }
    }
}