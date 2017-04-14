using System;
using System.Globalization;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerTimeColumn: SqlServerColumn
    {
        public SqlServerTimeColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Time, isNullable, false, def)
        {
            Type = ColumnTypeName.Time;
        }

        public override string TypeToString()
        {
            return "time";
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
            var timeSpan = (TimeSpan)value;
            return timeSpan.ToString("c", CultureInfo.InvariantCulture);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return TimeSpan.ParseExact(value, "c", CultureInfo.InvariantCulture);
        }

        public override Type GetDotNetType()
        {
            return typeof(TimeSpan);
        }
    }
}