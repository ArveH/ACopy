using ADatabase.Exceptions;

namespace ADatabase.Extensions
{
    public static class StringCustomExtensions
    {
        public static ColumnTypeName ColumnTypeName(this string str)
        {
            switch (str)
            {
                case "varchar":
                case "varchar2":
                    return ADatabase.ColumnTypeName.Varchar;
                case "nvarchar":
                case "nvarchar2":
                    return ADatabase.ColumnTypeName.NVarchar;
                case "char":
                    return ADatabase.ColumnTypeName.Char;
                case "nchar":
                    return ADatabase.ColumnTypeName.NChar;
                case "longtext":
                case "clob":
                    return ADatabase.ColumnTypeName.LongText;
                case "nlongtext":
                    return ADatabase.ColumnTypeName.NLongText;
                case "bit":
                case "bool":
                    return ADatabase.ColumnTypeName.Bool;
                case "int8":
                case "tinyint":
                    return ADatabase.ColumnTypeName.Int8;
                case "int16":
                case "smallint":
                    return ADatabase.ColumnTypeName.Int16;
                case "int":
                    return ADatabase.ColumnTypeName.Int;
                case "bigint":
                case "int64":
                    return ADatabase.ColumnTypeName.Int64;
                case "money":
                    return ADatabase.ColumnTypeName.Money;
                case "dec":
                case "number":
                    return ADatabase.ColumnTypeName.Dec;
                case "float":
                    return ADatabase.ColumnTypeName.Float;
                case "date":
                case "datetime":
                    return ADatabase.ColumnTypeName.DateTime;
                case "datetime2":
                    return ADatabase.ColumnTypeName.DateTime2;
                case "guid":
                case "uniqueidentity":
                    return ADatabase.ColumnTypeName.Guid;
                case "blob":
                    return ADatabase.ColumnTypeName.Blob;
                case "varbinary":
                case "raw":
                    return ADatabase.ColumnTypeName.Raw;
                case "identity":
                    return ADatabase.ColumnTypeName.Identity;
            }

            throw new ADatabaseException($"Copy program doesn't have a representation for ACopy column type {str}");
        }

        public static string AddParameters(this string str)
        {
            switch (str)
            {
                case "VARCHAR2":
                case "VARCHAR":
                case "varchar":
                case "CHAR":
                case "char":
                case "RAW":
                case "varbinary":
                case "NVARCHAR2":
                case "NVARCHAR":
                case "nvarchar":
                case "NCHAR":
                case "nchar":
                    return str.ToLower() + "(@Length)";
                case "LONG RAW":
                    return "longraw(@Length)";
                case "NUMBER":
                case "NUMERIC":
                case "DECIMAL":
                    return "number(@Prec, @Scale)";
                case "dec":
                case "decimal":
                    return "dec(@Prec, @Scale)";
                default:
                    return str.ToLower();
            }
        }
    }
}