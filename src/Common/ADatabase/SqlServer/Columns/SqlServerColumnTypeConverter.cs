using ADatabase.Exceptions;

namespace ADatabase.SqlServer.Columns
{
    public static class SqlServerColumnTypeConverter
    {
        public static ColumnTypeName GetColumnTypeFromNativeType(string nativeType, ref int length, int prec, int scale)
        {
            switch (nativeType)
            {
                case "varchar":
                    if (length == -1)
                    {
                        return ColumnTypeName.LongText;
                    }
                    return ColumnTypeName.Varchar;
                case "char":
                    return ColumnTypeName.Char;
                case "nvarchar":
                    if (length == -1)
                    {
                        return ColumnTypeName.LongText;
                    }
                    length /= 2; // Length is in bytes, but we want it in characters
                    return ColumnTypeName.String;
                case "bit":
                    return ColumnTypeName.Bool;
                case "tinyint":
                    return ColumnTypeName.Int8;
                case "smallint":
                    return ColumnTypeName.Int16;
                case "int":
                    return ColumnTypeName.Int;
                case "bigint":
                    return ColumnTypeName.Int64;
                case "decimal":
                    if (prec == 28 && scale == 3)
                    {
                        return ColumnTypeName.Money;
                    }
                    return ColumnTypeName.Float;
                case "datetime":
                case "datetime2":
                    return ColumnTypeName.DateTime;
                case "uniqueidentifier":
                    return ColumnTypeName.Guid;
                case "varbinary":
                    if (length == -1)
                    {
                        return ColumnTypeName.Blob;
                    }
                    break;
            }

            throw new ADatabaseException(string.Format("Copy program doesn't handle columns of type {0}", nativeType));
        }

    }
}