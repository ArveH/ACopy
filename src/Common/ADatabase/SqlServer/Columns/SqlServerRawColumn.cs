using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerRawColumn : SqlServerColumn
    {
        public SqlServerRawColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Blob, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "varbinary(max)";
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
            throw new NotImplementedException("Column.ToFile for RAW");
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException("Column.ToInternalType for RAW");
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}