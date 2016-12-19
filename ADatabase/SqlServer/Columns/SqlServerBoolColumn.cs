using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerBoolColumn : SqlServerInt32Column
    {
        public SqlServerBoolColumn(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnType.Bool;
        }

        public override string TypeToString()
        {
            return string.Format("bit");
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