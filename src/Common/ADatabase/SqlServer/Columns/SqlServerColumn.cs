using System;
using System.Collections.Generic;

namespace ADatabase.SqlServer.Columns
{
    public abstract class SqlServerColumn : IColumn
    {
        protected SqlServerColumn(string name, ColumnTypeName type, bool isNullable, string def)
        {
            Name = name;
            Type = type;
            IsNullable = isNullable;
            _default = def;
            _details = new Dictionary<string, object>();
        }

        public string Name { get; set; }
        public ColumnTypeName Type { get; set; }
        public bool IsNullable { get; set; }

        private string _default;

        public virtual string Default
        {
            get { return _default; } set { _default = value; }
        }

        private readonly Dictionary<string, object> _details;
        public Dictionary<string, object> Details
        {
            get { return _details; }
        }

        public abstract string TypeToString();
        public abstract string GetColumnDefinition();
        public abstract string ToString(object value);
        public abstract object ToInternalType(string value);
        public abstract Type GetDotNetType();
    }
}