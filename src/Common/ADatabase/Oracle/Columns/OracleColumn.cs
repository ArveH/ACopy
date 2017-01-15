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

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ColumnTypeName _type;
        public ColumnTypeName Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _isNullable;
        public bool IsNullable
        {
            get { return _isNullable; }
            set { _isNullable = value; }
        }

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
            var notNullConstraint = _isNullable ? "null " : "not null ";

            return string.Format("{0} {1}{2}", TypeToString(), defaultValue, notNullConstraint);
        }

        public abstract string TypeToString();
        public abstract string ToString(object value);
        public abstract object ToInternalType(string value);
        public abstract Type GetDotNetType();
    }
}