using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerFloatColumn : SqlServerIntColumn
    {
        private readonly string _typeToString;

        public SqlServerFloatColumn(string name, int prec, bool isNullable, string def)
            : base(name, isNullable, false, def)
        {
            if (prec > 0)
            {
                Type = ColumnTypeName.Float;
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
            return typeof(double);
        }
    }
}