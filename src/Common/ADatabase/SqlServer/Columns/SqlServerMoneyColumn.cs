using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerMoneyColumn : SqlServerInt32Column
    {
        public SqlServerMoneyColumn(string name, bool isNullable, string def)
            : base(name, isNullable, false, def)
        {
            Type = ColumnTypeName.Money;
        }

        public override string TypeToString()
        {
            return "money";
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