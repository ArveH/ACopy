using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleTimestampColumn: OracleDateColumn
    {
        private readonly string _typeToString;

        public OracleTimestampColumn(string name, int scale, bool isNullable, string def)
            : base(name, isNullable, ConvertNativeDateToKeyword(def))
        {
            if (scale > 0)
            {
                Type = ColumnTypeName.DateTime2;
                Details["Scale"] = scale;
                _typeToString = $"timestamp({scale})";
            }
            else
            {
                _typeToString = "timestamp";
            }
        }

        public override string ToString(object value)
        {
            return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss.fffffff");
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
        }

        public override string TypeToString()
        {
            return _typeToString;
        }

    }
}