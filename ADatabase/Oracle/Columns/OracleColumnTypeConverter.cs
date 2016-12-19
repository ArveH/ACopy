using ADatabase.Exceptions;

namespace ADatabase.Oracle.Columns
{
    public static class OracleColumnTypeConverter
    {
        public static ColumnType GetColumnTypeFromNativeType(string nativeType, int length, int prec, int scale)
        {
            switch (nativeType)
            {
                case "VARCHAR2":
                case "VARCHAR":
                case "CHAR":
                    return ColumnType.Varchar;
                case "CLOB":
                    return ColumnType.LongText;
                case "INTEGER":
                    return ColumnType.Int;
                case "NUMBER":
                    if (scale == 0)
                    {
                        switch (prec)
                        {
                            case 1:
                                return ColumnType.Bool;
                            case 3:
                                return ColumnType.Int8;
                            case 5:
                                return ColumnType.Int16;
                            case 20:
                                return ColumnType.Int64;
                            default:
                                return ColumnType.Int;
                        }
                    }
                    if ( (scale == 2 && prec == 18) || (scale == 3 && prec == 30) )
                    {
                        return ColumnType.Money;
                    }
                    return ColumnType.Float;
                case "FLOAT":
                    return ColumnType.Float;
                case "DATE":
                    return ColumnType.DateTime;
                case "RAW":
                    if (length == 16 || length == 32 || length == 17 || length == 34)
                    {
                        return ColumnType.Guid;
                    }
                    break;
                case "BLOB":
                case "LONG RAW":
                    return ColumnType.Raw;
            }

            throw new ADatabaseException(string.Format("Copy program doesn't handle columns of type {0}", nativeType));
        }

    }
}