using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerFloatColumn : SqlServerInt32Column
    {
        public SqlServerFloatColumn(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Details["Prec"] = 28;
            Details["Scale"] = 8;
            Type = ColumnTypeName.Float;
        }

        public override string TypeToString()
        {
            return "dec(28,8)";
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