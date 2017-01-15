using System;
using System.Collections.Generic;
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
                case ColumnTypeName.String:
                    return new OracleVarchar2Column(name, length, isNullable, def);
                case ColumnTypeName.LongText:
                    return new OracleLongTextColumn(name, isNullable, def, collation);
                case ColumnTypeName.Int:
                    return new OracleIntColumn(name, isNullable, def);
                case ColumnTypeName.Bool:
                    return new OracleBoolColumn(name, isNullable, def);
                case ColumnTypeName.Int8:
                    return new OracleInt8Column(name, isNullable, def);
                case ColumnTypeName.Int16:
                    return new OracleInt16Column(name, isNullable, def);
                case ColumnTypeName.Int64:
                    return new OracleInt64Column(name, isNullable, def);
                case ColumnTypeName.Money:
                    return new OracleMoneyColumn(name, isNullable, def);
                case ColumnTypeName.Float:
                    return new OracleFloatColumn(name, isNullable, def);
                case ColumnTypeName.DateTime:
                    return new OracleDatetimeColumn(name, isNullable, def);
                case ColumnTypeName.Guid:
                    return new OracleGuidColumn(name, length==0?16:length, isNullable, def);
                case ColumnTypeName.Raw:
                    return new OracleRawColumn(name, isNullable, def);
            }

            throw new NotImplementedException();
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