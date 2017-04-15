using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleNumberColumn: OracleColumn
    {
        private readonly string _typeToString;

        public OracleNumberColumn(string name, ColumnTypeName columnTypeName, int prec, int scale, bool isNullable, string def)
            : base(name, columnTypeName, isNullable, def)
        {
            Details["Prec"] = prec;
            Details["Scale"] = scale;
            if (prec == 0 && scale == 0) _typeToString = "number";
            else if (prec == 0 && scale != 0) _typeToString = $"number(*,{scale})";
            else if (prec != 0 && scale == 0) _typeToString = $"number({prec})";
            else _typeToString = $"number({prec},{scale})";
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
            return typeof(decimal);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}