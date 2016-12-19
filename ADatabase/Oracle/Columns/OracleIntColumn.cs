using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleIntColumn: OracleNumberColumn
    {
        public OracleIntColumn(string name, bool isNullable, string def)
            : base(name, ColumnType.Int, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "number(15)";
        }

        public override Type GetDotNetType()
        {
            return typeof(int);
        }
    }
}
