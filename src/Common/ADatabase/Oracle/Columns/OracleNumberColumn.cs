using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleNumberColumn: OracleColumn
    {
        private readonly string _typeToString;

        public OracleNumberColumn(string name, int prec, int scale, bool isNullable, string def)
            : base(name, ColumnTypeName.Dec, isNullable, def)
        {
            Details["Prec"] = prec;
            Details["Scale"] = scale;
            _typeToString = $"number({prec},{scale})";
        }

        public override string TypeToString()
        {
            return _typeToString;
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