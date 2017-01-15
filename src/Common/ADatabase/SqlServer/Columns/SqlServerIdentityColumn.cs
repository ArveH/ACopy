using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerIdentityColumn: SqlServerInt64Column
    {
        public SqlServerIdentityColumn(string name, bool isNullable, string def)
            : base(name, isNullable, def)
        {
            Type = ColumnTypeName.Identity;
            Details["Identity"] = true;
        }

        public override string TypeToString()
        {
            return "bigint identity unique";
        }

        public override string GetColumnDefinition()
        {
            return TypeToString();
        }

        public override string Default
        {
            // Identity columns doesn't have default values
            get { return ""; }
            set { base.Default = ""; }
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
    }
}
