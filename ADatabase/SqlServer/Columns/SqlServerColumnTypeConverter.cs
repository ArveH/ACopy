using ADatabase.Exceptions;

namespace ADatabase.SqlServer.Columns
{
    public static class SqlServerColumnTypeConverter
    {
        public static ColumnType GetColumnTypeFromNativeType(string nativeType, ref int length, int prec, int scale)
        {
            switch (nativeType)
            {
                case "varchar":
                    if (length == -1)
                    {
                        return ColumnType.LongText;
                    }
                    return ColumnType.Varchar;
                case "char":
                    return ColumnType.Char;
                case "nvarchar":
                    if (length == -1)
                    {
                        return ColumnType.LongText;
                    }
                    length /= 2; // Length is in bytes, but we want it in characters
                    return ColumnType.String;
                case "bit":
                    return ColumnType.Bool;
                case "tinyint":
                    return ColumnType.Int8;
                case "smallint":
                    return ColumnType.Int16;
                case "int":
                    return ColumnType.Int;
                case "bigint":
                    return ColumnType.Int64;
                case "decimal":
                    if (prec == 28 && scale == 3)
                    {
                        return ColumnType.Money;
                    }
                    return ColumnType.Float;
                case "datetime":
                case "datetime2":
                    return ColumnType.DateTime;
                case "uniqueidentifier":
                    return ColumnType.Guid;
                case "varbinary":
                    if (length == -1)
                    {
                        return ColumnType.Raw;
                    }
                    break;
            }

            throw new ADatabaseException(string.Format("Copy program doesn't handle columns of type {0}", nativeType));
        }

    }
}