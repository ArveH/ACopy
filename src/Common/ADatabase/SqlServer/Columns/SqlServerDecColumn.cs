using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerDecColumn: SqlServerInt32Column
    {
        private readonly string _typeToString;

        public SqlServerDecColumn(string name, int prec, int scale, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Details["Prec"] = prec;
            Details["Scale"] = scale;
            Type = ColumnTypeName.Float;
            _typeToString = $"dec({prec},{scale})";
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

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        public override Type GetDotNetType()
        {
            return typeof(decimal);
        }

    }
}