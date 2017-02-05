using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleBinaryFloatColumn: OracleColumn
    {
        public OracleBinaryFloatColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Float, isNullable, def)
        {
        }

        public override string ToString(object value)
        {
            return Convert.ToDouble(value).ToString("F8", CultureInfo.InvariantCulture);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        public override Type GetDotNetType()
        {
            return typeof(float);
        }

        public override string TypeToString()
        {
            return "binary_float";
        }
    }
}
