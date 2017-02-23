using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerBitColumn : SqlServerIntColumn
    {
        public SqlServerBitColumn(string name, bool isNullable, string def)
            : base(name, isNullable, false, def)
        {
            Type = ColumnTypeName.Bool;
        }

        public override string TypeToString()
        {
            return "bit";
        }

        public override string ToString(object value)
        {
            return (bool)value ? "1" : "0";
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return value.Length == 1 ? value == "1" : Convert.ToBoolean(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(bool);
        }
    }
}