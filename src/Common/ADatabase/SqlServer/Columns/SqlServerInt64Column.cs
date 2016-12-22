using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerInt64Column : SqlServerInt32Column
    {
        public SqlServerInt64Column(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnType.Int64;
        }

        public override string TypeToString()
        {
            return "bigint";
        }

        public override string ToString(object value)
        {
            return Convert.ToInt64(value).ToString();
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ToInt64(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(Int64);
        }
    }
}