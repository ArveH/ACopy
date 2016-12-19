using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerInt16Column : SqlServerInt32Column
    {
        public SqlServerInt16Column(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnType.Int16;
        }

        public override string TypeToString()
        {
            return "smallint";
        }

        public override string ToString(object value)
        {
            return Convert.ToInt16(value).ToString();
        }

        public override Type GetDotNetType()
        {
            return typeof(short);
        }
    }
}