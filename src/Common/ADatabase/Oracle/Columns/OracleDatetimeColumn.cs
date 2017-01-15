using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleDatetimeColumn : OracleColumn
    {
        public OracleDatetimeColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.DateTime, isNullable, ConvertNativeDateToKeyword(def))
        {
        }


        public override string TypeToString()
        {
            return "date";
        }

        protected override string ParseDefaultValue(string def)
        {
            switch (def)
            {
                case "MIN_DATE":
                    return "to_date('19000101 00:00:00', 'yyyymmdd hh24:mi:ss')";
                case "MAX_DATE":
                    return "to_date('20991231 23:59:59', 'yyyymmdd hh24:mi:ss')";
                case "TS2DAY(MAX_DATE)":
                    return "to_date('20991231 00:00:00', 'yyyymmdd hh24:mi:ss')";
                case "TODAY":
                    return "trunc(sysdate, 'dd')";
                case "NOW":
                    return "sysdate";
            }

            return def;
        }


        public override string Default
        {
            get
            {
                return base.Default;
            }
            set
            {
                base.Default = ConvertNativeDateToKeyword(value);
            }
        }

        private static string ConvertNativeDateToKeyword(string date)
        {
            if ((date.IndexOf("19000101", StringComparison.Ordinal) > 0 || date.IndexOf("1900 01 01", StringComparison.Ordinal) > 0))
            {
                return "MIN_DATE";
            }
            else if (date.IndexOf("2099", StringComparison.Ordinal) > 0 && date.IndexOf("59:59", StringComparison.Ordinal) > 0 && date.IndexOf("trunc", StringComparison.Ordinal) < 0)
            {
                return "MAX_DATE";
            }
            else if (date.IndexOf("2099", StringComparison.Ordinal) > 0)
            {
                return "TS2DAY(MAX_DATE)";
            }
            else if (date.IndexOf("trunc", StringComparison.Ordinal) >= 0 && date.IndexOf("sysdate", StringComparison.Ordinal) > 0)
            {
                return "TODAY";
            }
            else if (date.IndexOf("sysdate", StringComparison.Ordinal) >= 0)
            {
                return "NOW";
            }

            return date;
        }

        public override string ToString(object value)
        {
            return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss");
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override Type GetDotNetType()
        {
            return typeof(DateTime);
        }
    }
}