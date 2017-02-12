using System;
using System.Collections.Generic;
using ADatabase.Exceptions;
using ADatabase.SqlServer.Columns;

namespace ADatabase.SqlServer
{
    public class SqlServerColumnFactory : IColumnFactory
    {
        public IColumn CreateInstance(ColumnTypeName type, string name, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            switch (type)
            {
                case ColumnTypeName.Varchar:
                    return new SqlServerVarcharColumn(name, length, isNullable, def, collation);
                case ColumnTypeName.Char:
                    return new SqlServerCharColumn(name, length, isNullable, def, collation);
                case ColumnTypeName.NVarchar:
                    return new SqlServerNVarcharColumn(name, length, isNullable, def, collation);
                case ColumnTypeName.LongText:
                    return new SqlServerLongTextColumn(name, isNullable, def, collation);
                case ColumnTypeName.Int:
                    return new SqlServerInt32Column(name, isNullable, def);
                case ColumnTypeName.Bool:
                    return new SqlServerBoolColumn(name, isNullable, def);
                case ColumnTypeName.Int8:
                    return new SqlServerInt8Column(name, isNullable, def);
                case ColumnTypeName.Int16:
                    return new SqlServerInt16Column(name, isNullable, def);
                case ColumnTypeName.Int64:
                    return new SqlServerInt64Column(name, isNullable, def);
                case ColumnTypeName.Money:
                    return new SqlServerMoneyColumn(name, isNullable, def);
                case ColumnTypeName.Float:
                    return new SqlServerFloatColumn(name, isNullable, def);
                case ColumnTypeName.Dec:
                    return new SqlServerDecColumn(name, prec, scale, isNullable, def);
                case ColumnTypeName.DateTime:
                    return new SqlServerDatetimeColumn(name, isNullable, def);
                case ColumnTypeName.Guid:
                    return new SqlServerGuidColumn(name, isNullable, def);
                case ColumnTypeName.Blob:
                case ColumnTypeName.Raw:
                    return new SqlServerBlobColumn(name, length, isNullable, def);
                case ColumnTypeName.Identity:
                    return new SqlServerIdentityColumn(name, isNullable, def);
            }

            throw new AColumnTypeException($"Illegal type: {type}");
        }

        public IColumn CreateInstance(ColumnTypeName type, string name, int length, bool isNullable, string def, string collation)
        {
            return CreateInstance(type, name, length, 0, 0, isNullable, def, collation);
        }

        public IColumn CreateInstance(ColumnTypeName type, string name, bool isNullable, string def)
        {
            return CreateInstance(type, name, 0, 0, 0, isNullable, def, "");
        }

        public IColumn CreateInstance(ColumnTypeName columnType, string colName, bool isNullable, string def, Dictionary<string, object> details)
        {
            int length = 0;
            int prec = 0;
            int scale = 0;
            string collation = "";

            if (details.ContainsKey("Length"))
            {
                length = Convert.ToInt32(details["Length"]);
            }
            if (details.ContainsKey("Prec"))
            {
                prec = Convert.ToInt32(details["Prec"]);
            }
            if (details.ContainsKey("Scale"))
            {
                scale = Convert.ToInt32(details["Scale"]);
            }
            if (details.ContainsKey("Collation"))
            {
                collation = details["Collation"].ToString();
            }

            return CreateInstance(columnType, colName, length, prec, scale, isNullable, def, collation);
        }
    }
}