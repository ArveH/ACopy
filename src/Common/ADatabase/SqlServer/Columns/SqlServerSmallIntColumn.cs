using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerSmallIntColumn : SqlServerIntColumn
    {
        public SqlServerSmallIntColumn(string name, bool isNullable, bool isIdentity, string def)
            : base(name, isNullable, isIdentity, def)
        {
            Type = ColumnTypeName.Int16;
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