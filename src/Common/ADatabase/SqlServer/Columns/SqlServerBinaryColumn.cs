using System;
using System.Text;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerBinaryColumn : SqlServerColumn
    {
        private readonly string _typeString;
        public SqlServerBinaryColumn(string name, int length, bool isNullable, string def)
            : base(name, ColumnTypeName.Raw, isNullable, false, def)
        {
            Details["Length"] = length;
            _typeString =  $"binary({length})";
        }

        public override string TypeToString()
        {
            return _typeString;
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
                return DBNull.Value;
            }
            return Convert.FromBase64String(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}