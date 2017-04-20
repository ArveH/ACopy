using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerTimestampColumn: SqlServerColumn
    {
        public SqlServerTimestampColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Timestamp, isNullable, false, def)
        {
        }

        public override string TypeToString()
        {
            return "timestamp";
        }

        public override string GetColumnDefinition()
        {
            var defaultValue = "";
            if (!string.IsNullOrEmpty(Default))
            {
                defaultValue = $"default {Default} ";
            }
            var notNullConstraint = IsNullable ? "null " : "not null ";

            return $"{TypeToString()} {defaultValue}{notNullConstraint}";
        }

        public override string ToString(object value)
        {
            return Convert.ToBase64String((byte[])value);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.FromBase64String(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}