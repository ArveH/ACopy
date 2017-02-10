using ADatabase.Exceptions;

namespace ADatabase.Extensions
{
    public static class StringCustomExtensions
    {
        public static ColumnTypeName Oracle2ColumnTypeName(this string str)
        {
            switch (str)
            {
                case "varchar2":
                case "varchar":
                case "char":
                    return ColumnTypeName.Varchar;
                case "clob":
                    return ColumnTypeName.LongText;
                case "integer":
                    return ColumnTypeName.Int;
                case "number":
                    return ColumnTypeName.Dec;
                case "float":
                    return ColumnTypeName.Float;
                case "date":
                    return ColumnTypeName.DateTime;
                case "longraw":
                case "raw":
                    return ColumnTypeName.Raw;
                case "blob":
                    return ColumnTypeName.Blob;
            }

            throw new ADatabaseException($"Copy program doesn't have a representation for Oracle column type {str}");
        }

        public static ColumnTypeName ACopy2ColumnTypeName(this string str)
        {
            switch (str)
            {
                case "varchar":
                    return ColumnTypeName.Varchar;
                case "char":
                    return ColumnTypeName.Char;
                case "string":
                    return ColumnTypeName.String;
                case "longtext":
                    return ColumnTypeName.LongText;
                case "bool":
                    return ColumnTypeName.Bool;
                case "int8":
                    return ColumnTypeName.Int8;
                case "int16":
                    return ColumnTypeName.Int16;
                case "int":
                    return ColumnTypeName.Int;
                case "int64":
                    return ColumnTypeName.Int64;
                case "money":
                    return ColumnTypeName.Money;
                case "dec":
                    return ColumnTypeName.Dec;
                case "float":
                    return ColumnTypeName.Float;
                case "datetime":
                    return ColumnTypeName.DateTime;
                case "guid":
                    return ColumnTypeName.Guid;
                case "blob":
                    return ColumnTypeName.Blob;
                case "raw":
                    return ColumnTypeName.Raw;
                case "identity":
                    return ColumnTypeName.Identity;
            }

            throw new ADatabaseException($"Copy program doesn't have a representation for ACopy column type {str}");
        }

        public static string ToOracleTypeWithParameters(this string str)
        {
            switch (str)
            {
                case "VARCHAR2":
                case "VARCHAR":
                case "CHAR":
                case "RAW":
                    return str.ToLower() + "(@Length)";
                case "LONG RAW":
                    return "longraw(@Length)";
                case "NUMBER":
                    return str.ToLower() + "(@Prec, @Scale)";
                default:
                    return str.ToLower();
            }
        }
    }
}