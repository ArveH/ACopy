using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerVarBinaryColumn : SqlServerColumn
    {
        private readonly string _typeString;
        public SqlServerVarBinaryColumn(string name, int length, bool isNullable, string def)
            : base(name, ColumnTypeName.Blob, isNullable, false, def)
        {
            Details["Length"] = length;
            _typeString = length == 0 || length == -1 ? "varbinary(max)" : $"varbinary({length})";
        }

        public override string TypeToString()
        {
            return _typeString;
        }

        public override string GetColumnDefinition()
        {
            string defaultValue = "";
            if (!string.IsNullOrEmpty(Default))
            {
                defaultValue = string.Format("default {0} ", Default);
            }
            string notNullConstraint = IsNullable ? "null " : "not null ";

            return string.Format("{0} {1}{2}", TypeToString(), defaultValue, notNullConstraint);
        }

        public override string ToString(object value)
        {
            throw new NotImplementedException("Column.ToFile for BLOB");
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException("Column.ToInternalType for BLOB");
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}