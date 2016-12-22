using System;
using System.Collections.Generic;
using ADatabase.Oracle.Columns;

namespace ADatabase.Oracle
{
    public class OracleColumnFactory : IColumnFactory
    {
        public IColumn CreateInstance(ColumnType type, string name, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            switch (type)
            {
                case ColumnType.Varchar:
                case ColumnType.Char:
                case ColumnType.String:
                    return new OracleVarchar2Column(name, length, isNullable, def);
                case ColumnType.LongText:
                    return new OracleLongTextColumn(name, isNullable, def, collation);
                case ColumnType.Int:
                    return new OracleIntColumn(name, isNullable, def);
                case ColumnType.Bool:
                    return new OracleBoolColumn(name, isNullable, def);
                case ColumnType.Int8:
                    return new OracleInt8Column(name, isNullable, def);
                case ColumnType.Int16:
                    return new OracleInt16Column(name, isNullable, def);
                case ColumnType.Int64:
                    return new OracleInt64Column(name, isNullable, def);
                case ColumnType.Money:
                    return new OracleMoneyColumn(name, isNullable, def);
                case ColumnType.Float:
                    return new OracleFloatColumn(name, isNullable, def);
                case ColumnType.DateTime:
                    return new OracleDatetimeColumn(name, isNullable, def);
                case ColumnType.Guid:
                    return new OracleGuidColumn(name, length==0?16:length, isNullable, def);
                case ColumnType.Raw:
                    return new OracleRawColumn(name, isNullable, def);
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