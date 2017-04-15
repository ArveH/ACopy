using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleFloatColumn: OracleColumn
    {
        private readonly string _typeToString;

        public OracleFloatColumn(string name, int prec, bool isNullable, string def)
            : base(name, ColumnTypeName.Float, isNullable, def)
        {
            if (prec > 0)
            {
                Details["Prec"] = prec;
                _typeToString = $"float({prec})";
            }
            else
            {
                _typeToString = "float";
            }
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
            return typeof(double);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}