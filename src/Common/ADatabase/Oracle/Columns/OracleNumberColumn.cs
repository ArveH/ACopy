using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public abstract class OracleNumberColumn: OracleColumn
    {
        public OracleNumberColumn(string name, ColumnTypeName type, bool isNullable, string def)
            : base(name, type, isNullable, def)
        {
        }

        public override string ToString(object value)
        {
            return Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture);
        }

        public override Type GetDotNetType()
        {
            return typeof(Decimal);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return Decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}