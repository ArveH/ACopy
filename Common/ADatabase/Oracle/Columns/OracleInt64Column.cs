using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleInt64Column: OracleNumberColumn
    {
        public OracleInt64Column(string name, bool isNullable, string def)
            : base(name, ColumnType.Int64, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "number(20)";
        }

        public override Type GetDotNetType()
        {
            return typeof(Int64);
        }
    }
}
