using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerFloatColumn : SqlServerInt32Column
    {
        private readonly string _typeToString;

        public SqlServerFloatColumn(string name, int length, bool isNullable, string def)
            : base(name, isNullable, false, def)
        {
            if (length > 0)
            {
                Type = ColumnTypeName.Float;
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