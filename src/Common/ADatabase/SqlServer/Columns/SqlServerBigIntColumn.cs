using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerBigIntColumn : SqlServerIntColumn
    {
        public SqlServerBigIntColumn(string name, bool isNullable, bool isIdentity, string def)
            : base(name, isNullable, isIdentity, def)
        {
            Type = ColumnTypeName.Int64;
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
            return typeof(long);
        }
    }
}