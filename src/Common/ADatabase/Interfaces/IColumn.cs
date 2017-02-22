using System;
using System.Collections.Generic;

namespace ADatabase
{
    public interface IColumn
    {
        string Name { get; set; }
        ColumnTypeName Type { get; set; }
        Dictionary<string, object> Details { get; }
        bool IsNullable { get; set; }
        bool IsIdentity { get; }
        string Default { get; set; }
        string TypeToString();
        string GetColumnDefinition();
        string ToString(object value);
        object ToInternalType(string value);
        Type GetDotNetType();
    }
}