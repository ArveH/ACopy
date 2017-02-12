using System;
using System.Collections.Generic;
using ADatabase.Exceptions;
using ADatabase.Oracle.Columns;

namespace ADatabase.Oracle
{
    public class OracleColumnFactory : IColumnFactory
    {
        public IColumn CreateInstance(ColumnTypeName type, string name, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            switch (type)
            {
                case ColumnTypeName.Varchar:
                case ColumnTypeName.Char:
                case ColumnTypeName.NVarchar:
                case ColumnTypeName.NChar:
                    return new OracleVarchar2Column(name, length, isNullable, def);
                case ColumnTypeName.LongText:
                    return new OracleClobColumn(name, isNullable, def, collation);
                case ColumnTypeName.Bool:
                case ColumnTypeName.Int8:
                case ColumnTypeName.Int16:
                case ColumnTypeName.Int:
                case ColumnTypeName.Int64:
                case ColumnTypeName.Money:
                case ColumnTypeName.Float:
                case ColumnTypeName.Dec:
                    return new OracleNumberColumn(name, type, prec, scale, isNullable, def);
                case ColumnTypeName.DateTime:
                    return new OracleDateColumn(name, isNullable, def);
                case ColumnTypeName.Guid:
                    return new OracleRawColumn(name, 16, isNullable, def);
                case ColumnTypeName.Raw:
                    return new OracleRawColumn(name, length, isNullable, def);
                case ColumnTypeName.Blob:
                    return new OracleBlobColumn(name, isNullable, def);
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