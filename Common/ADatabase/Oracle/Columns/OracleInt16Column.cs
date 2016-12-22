using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleInt16Column: OracleNumberColumn
    {
        public OracleInt16Column(string name, bool isNullable, string def)
            : base(name, ColumnType.Int16, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "number(5)";
        }

        public override Type GetDotNetType()
        {
            return typeof(short);
        }
    }
}
