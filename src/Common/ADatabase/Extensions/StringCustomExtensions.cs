using System;
using System.Text.RegularExpressions;
using ADatabase.Exceptions;

namespace ADatabase.Extensions
{
    public static class StringCustomExtensions
    {
        public static ColumnTypeName Oracle2ColumnTypeName(this string str)
        {
            switch (str)
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
                    return ColumnTypeName.Dec;
                case "FLOAT":
                    return ColumnTypeName.Float;
                case "DATE":
                    return ColumnTypeName.DateTime;
                case "LONG RAW":
                case "RAW":
                    return ColumnTypeName.Raw;
                case "BLOB":
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
                case "LONG RAW":
                case "RAW":
                    return str.ToLower() + "(@Length)";
                case "NUMBER":
                    return str.ToLower() + "(@Prec, @Scale)";
                default:
                    return str.ToLower();
            }
        }
    }
}