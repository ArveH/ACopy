using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleFloatColumn : OracleNumberColumn
    {
        public OracleFloatColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Float, isNullable, def)
        {
            Details["Prec"] = 30;
            Details["Scale"] = 8;
        }

        public override string ToString(object value)
        {
            return Convert.ToDecimal(value).ToString("F8", CultureInfo.InvariantCulture);
        }

        public override string TypeToString()
        {
            return "number(30,8)";
        }
    }
}
