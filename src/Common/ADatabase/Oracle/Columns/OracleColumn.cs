using System;
using System.Collections.Generic;

namespace ADatabase.Oracle.Columns
{
    public abstract class OracleColumn : IColumn
    {
        protected OracleColumn(string name, ColumnTypeName type, bool isNullable, string def)
        {
            Name = name;
            Type = type;
            IsNullable = isNullable;
            _default = def;
            Details = new Dictionary<string, object>();
        }

        public string Name { get; set; }
        public ColumnTypeName Type { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; } = false;

        private string _default;
        public virtual string Default
        {
            get { return _default; }
            set
            {
                _default = value;
            }
        }

        public Dictionary<string, object> Details { get; protected set; }

        protected virtual string ParseDefaultValue(string def)
        {
            return def;
        }

        public string GetColumnDefinition()
        {
            var defaultValue = "";
            if (!string.IsNullOrEmpty(_default))
            {
                defaultValue = string.Format("default {0} ", ParseDefaultValue(_default));
            }
            var notNullConstraint = IsNullable ? "null " : "not null ";

            return string.Format("{0} {1}{2}", TypeToString(), defaultValue, notNullConstraint);
        }

        public abstract string TypeToString();
        public abstract string ToString(object value);
        public abstract object ToInternalType(string value);
        public abstract Type GetDotNetType();
    }
}