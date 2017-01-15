using System.Collections.Generic;

namespace ADatabase
{
    public interface IColumnFactory
    {
        IColumn CreateInstance(ColumnTypeName type, string name, int length, int prec, int scale, bool isNullable, string def, string collation);
        IColumn CreateInstance(ColumnTypeName type, string name, int length, bool isNullable, string def, string collation);
        IColumn CreateInstance(ColumnTypeName columnType, string colName, bool isNullable, string def);
        IColumn CreateInstance(ColumnTypeName columnType, string colName, bool isNullable, string def, Dictionary<string, object> details);
    }
}