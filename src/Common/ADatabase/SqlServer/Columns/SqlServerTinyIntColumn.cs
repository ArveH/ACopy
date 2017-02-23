using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerTinyIntColumn : SqlServerIntColumn
    {
        public SqlServerTinyIntColumn(string name, bool isNullable, bool isIdentity, string def)
            : base(name, isNullable, isIdentity, def)
        {
            Type = ColumnTypeName.Int8;
        }

        public override string TypeToString()
        {
            return "tinyint";
        }

        public override string ToString(object value)
        {
            return Convert.ToByte(value).ToString();
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ToByte(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(byte);
        }
    }
}