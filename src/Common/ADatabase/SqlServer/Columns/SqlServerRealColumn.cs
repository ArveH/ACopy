using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerRealColumn : SqlServerIntColumn
    {
        public SqlServerRealColumn(string name, bool isNullable, string def)
            : base(name, isNullable, false, def)
        {
            Type = ColumnTypeName.BinaryFloat;
        }

        public override string TypeToString()
        {
            return "real";
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