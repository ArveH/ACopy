using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerMoneyColumn : SqlServerInt32Column
    {
        public SqlServerMoneyColumn(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Details["Prec"] = 28;
            Details["Scale"] = 3;
            Type = ColumnType.Money;
        }

        public override string TypeToString()
        {
            return "dec(28,3)";
        }

        public override string ToString(object value)
        {
            return Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return Decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        public override Type GetDotNetType()
        {
            return typeof(Decimal);
        }
    }
}