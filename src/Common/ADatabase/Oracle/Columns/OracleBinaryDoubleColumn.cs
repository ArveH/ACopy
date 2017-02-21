using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleBinaryDoubleColumn: OracleColumn
    {
        public OracleBinaryDoubleColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Double,  isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "binary_double";
        }

        public override string ToString(object value)
        {
            // The # removes trailing zero. Will round up last number if more than 8 decimals. 
            return Convert.ToDouble(value).ToString("0.########", CultureInfo.InvariantCulture);
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