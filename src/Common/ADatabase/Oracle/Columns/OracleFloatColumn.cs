using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleFloatColumn: OracleColumn
    {
        private readonly string _typeToString;

        public OracleFloatColumn(string name, int length, bool isNullable, string def)
            : base(name, ColumnTypeName.Float, isNullable, def)
        {
            if (length > 0)
            {
                Details["Length"] = length;
                _typeToString = $"float({length})";
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
            // The # removes trailing zero. Will round up last number if more than 8 decimals. 
            return Convert.ToDecimal(value).ToString("0.########", CultureInfo.InvariantCulture);
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