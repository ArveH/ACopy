using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerInt8Column : SqlServerInt32Column
    {
        public SqlServerInt8Column(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnType.Int8;
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