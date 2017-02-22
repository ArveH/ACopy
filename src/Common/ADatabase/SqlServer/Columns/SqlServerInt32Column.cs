using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerInt32Column : SqlServerColumn
    {
        public SqlServerInt32Column(string name, bool isNullable, bool isIdentity, string def)
            : base(name, ColumnTypeName.Int, isNullable, isIdentity, AdjustDefaultValue(def))
        {
        }

        public override string TypeToString()
        {
            return "int";
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

        public override string Default
        {
            get { return base.Default; }
            set
            {
                base.Default = AdjustDefaultValue(value);
            }
        }

        private static string AdjustDefaultValue(string def)
        {
            if (def.Length >= 4)
            {
                return def.Substring(2, def.Length - 4);
            }

            return def;
        }

        public override string ToString(object value)
        {
            return Convert.ToInt32(value).ToString();
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ToInt32(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(int);
        }
    }
}