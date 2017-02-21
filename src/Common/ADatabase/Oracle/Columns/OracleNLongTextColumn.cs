using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleNLongTextColumn: OracleVarchar2Column
    {
        public OracleNLongTextColumn(string name, bool isNullable, string def)
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