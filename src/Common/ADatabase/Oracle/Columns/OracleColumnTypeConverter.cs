using ADatabase.Exceptions;

namespace ADatabase.Oracle.Columns
{
    public static class OracleColumnTypeConverter
    {
        public static ColumnTypeName GetColumnTypeFromNativeType(string nativeType, int length, int prec, int scale)
        {
            switch (nativeType)
            {
                case "VARCHAR2":
                case "VARCHAR":
                case "CHAR":
                    return ColumnTypeName.Varchar;
                case "CLOB":
                    return ColumnTypeName.LongText;
                case "INTEGER":
                    return ColumnTypeName.Int;
                case "NUMBER":
                    if (scale == 0)
                    {
                        switch (prec)
                        {
                            case 1:
                                return ColumnTypeName.Bool;
                            case 3:
                                return ColumnTypeName.Int8;
                            case 5:
                                return ColumnTypeName.Int16;
                            case 20:
                                return ColumnTypeName.Int64;
                            default:
                                return ColumnTypeName.Int;
                        }
                    }
                    if ( (scale == 2 && prec == 18) || (scale == 3 && prec == 30) )
                    {
                        return ColumnTypeName.Money;
                    }
                    return ColumnTypeName.Float;
                case "FLOAT":
                    return ColumnTypeName.Float;
                case "DATE":
                    return ColumnTypeName.DateTime;
                case "RAW":
                    if (length == 16 || length == 32 || length == 17 || length == 34)
                    {
                        return ColumnTypeName.Guid;
                    }
                    break;
                case "BLOB":
                case "LONG RAW":
                    return ColumnTypeName.Raw;
            }

            throw new ADatabaseException(string.Format("Copy program doesn't handle columns of type {0}", nativeType));
        }

    }
}