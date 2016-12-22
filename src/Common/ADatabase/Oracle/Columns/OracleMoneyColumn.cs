using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleMoneyColumn : OracleNumberColumn
    {
        public OracleMoneyColumn(string name, bool isNullable, string def)
            : base(name, ColumnType.Money, isNullable, def)
        {
        }

        public override string ToString(object value)
        {
            return Convert.ToDecimal(value).ToString("F3", CultureInfo.InvariantCulture);
        }

        public override string TypeToString()
        {
            return "number(30,3)";
        }
    }
}
