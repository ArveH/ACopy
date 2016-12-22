using System;
using System.Collections.Generic;
using ADatabase.SqlServer.Columns;

namespace ADatabase.SqlServer
{
    public class SqlServerColumnFactory : IColumnFactory
    {
        public IColumn CreateInstance(ColumnType type, string name, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            switch (type)
            {
                case ColumnType.Varchar:
                    return new SqlServerVarcharColumn(name, length, isNullable, def, collation);
                case ColumnType.Char:
                    return new SqlServerCharColumn(name, length, isNullable, def, collation);
                case ColumnType.String:
                    return new SqlServerStringColumn(name, length, isNullable, def, collation);
                case ColumnType.LongText:
                    return new SqlServerLongTextColumn(name, isNullable, def, collation);
                case ColumnType.Int:
                    return new SqlServerInt32Column(name, isNullable, def);
                case ColumnType.Bool:
                    return new SqlServerBoolColumn(name, isNullable, def);
                case ColumnType.Int8:
                    return new SqlServerInt8Column(name, isNullable, def);
                case ColumnType.Int16:
                    return new SqlServerInt16Column(name, isNullable, def);
                case ColumnType.Int64:
                    return new SqlServerInt64Column(name, isNullable, def);
                case ColumnType.Money:
                    return new SqlServerMoneyColumn(name, isNullable, def);
                case ColumnType.Float:
                    return new SqlServerFloatColumn(name, isNullable, def);
                case ColumnType.DateTime:
                    return new SqlServerDatetimeColumn(name, isNullable, def);
                case ColumnType.Guid:
                    return new SqlServerGuidColumn(name, isNullable, def);
                case ColumnType.Raw:
                    return new SqlServerRawColumn(name, isNullable, def);
                case ColumnType.Identity:
                    return new SqlServerIdentityColumn(name, isNullable, def);
            }

            throw new NotImplementedException();
        }

        public IColumn CreateInstance(ColumnType type, string name, int length, bool isNullable, string def, string collation)
        {
            return CreateInstance(type, name, length, 0, 0, isNullable, def, collation);
        }

        public IColumn CreateInstance(ColumnType type, string name, bool isNullable, string def)
        {
            return CreateInstance(type, name, 0, 0, 0, isNullable, def, "");
        }

        public IColumn CreateInstance(ColumnType columnType, string colName, bool isNullable, string def, Dictionary<string, object> details)
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