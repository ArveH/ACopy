using System;
using ADatabase.Exceptions;

namespace ADatabase.Extensions
{
    public static class StringCustomExtensions
    {
        public static TEnumType ConverToEnum<TEnumType>(this string enumValue)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), enumValue);
        }

        public static ColumnTypeName ColumnTypeName(this string str)
        {
            switch (str)
            {
                case "bigint":
                    return ADatabase.ColumnTypeName.Int64;
                case "binary_float":
                case "binaryfloat":
                    return ADatabase.ColumnTypeName.BinaryFloat;
                case "binary_double":
                case "binarydouble":
                    return ADatabase.ColumnTypeName.BinaryDouble;
                case "bit":
                    return ADatabase.ColumnTypeName.Bool;
                case "blob":
                    return ADatabase.ColumnTypeName.Blob;
                case "bool":
                    return ADatabase.ColumnTypeName.Bool;
                case "char":
                    return ADatabase.ColumnTypeName.Char;
                case "clob":
                    return ADatabase.ColumnTypeName.LongText;
                case "date":
                    return ADatabase.ColumnTypeName.Date;
                case "datetime":
                    return ADatabase.ColumnTypeName.DateTime;
                case "datetime2":
                    return ADatabase.ColumnTypeName.DateTime2;
                case "dec":
                    return ADatabase.ColumnTypeName.Dec;
                case "float":
                    return ADatabase.ColumnTypeName.Float;
                case "guid":
                    return ADatabase.ColumnTypeName.Guid;
                case "image":
                    return ADatabase.ColumnTypeName.OldBlob;
                case "int":
                    return ADatabase.ColumnTypeName.Int;
                case "int16":
                    return ADatabase.ColumnTypeName.Int16;
                case "int64":
                    return ADatabase.ColumnTypeName.Int64;
                case "int8":
                    return ADatabase.ColumnTypeName.Int8;
                case "longtext":
                    return ADatabase.ColumnTypeName.LongText;
                case "long":
                    return ADatabase.ColumnTypeName.OldText;
                case "longraw":
                    return ADatabase.ColumnTypeName.OldBlob;
                case "money":
                    return ADatabase.ColumnTypeName.Money;
                case "nchar":
                    return ADatabase.ColumnTypeName.NChar;
                case "nclob":
                case "nlongtext":
                    return ADatabase.ColumnTypeName.NLongText;
                case "number":
                    return ADatabase.ColumnTypeName.Dec;
                case "nvarchar":
                case "nvarchar2":
                    return ADatabase.ColumnTypeName.NVarchar;
                case "oldblob":
                    return ADatabase.ColumnTypeName.OldBlob;
                case "oldtext":
                    return ADatabase.ColumnTypeName.OldText;
                case "raw":
                    return ADatabase.ColumnTypeName.Raw;
                case "real":
                    return ADatabase.ColumnTypeName.BinaryFloat;
                case "smalldatetime":
                    return ADatabase.ColumnTypeName.SmallDateTime;
                case "smallint":
                    return ADatabase.ColumnTypeName.Int16;
                case "smallmoney":
                    return ADatabase.ColumnTypeName.SmallMoney;
                case "text":
                    return ADatabase.ColumnTypeName.OldText;
                case "time":
                    return ADatabase.ColumnTypeName.Time;
                case "timestamp":
                    return ADatabase.ColumnTypeName.Timestamp;
                case "tinyint":
                    return ADatabase.ColumnTypeName.Int8;
                case "uniqueidentity":
                    return ADatabase.ColumnTypeName.Guid;
                case "varbinary":
                    return ADatabase.ColumnTypeName.Raw;
                case "varchar":
                case "varchar2":
                    return ADatabase.ColumnTypeName.Varchar;
            }

            throw new ADatabaseException($"Copy program doesn't have a representation for ACopy column type {str}");
        }

        public static string AddParameters(this string str)
        {
            switch (str)
            {
                case "binary":
                    return "binary(@Length)";
                case "datetime2":
                    return "datetime2(@Scale)";
                case "CHAR":
                case "char":
                case "NCHAR":
                case "nchar":
                case "NVARCHAR2":
                case "NVARCHAR":
                case "nvarchar":
                case "RAW":
                case "VARCHAR2":
                case "VARCHAR":
                case "varchar":
                case "varbinary":
                    return str.ToLower() + "(@Length)";
                case "FLOAT":
                case "float":
                    return "float(@Prec)";
                case "LONG RAW":
                    return "longraw(@Length)";
                case "NUMBER":
                case "NUMERIC":
                case "DECIMAL":
                    return "number(@Prec, @Scale)";
                case "dec":
                    return "dec(@Prec, @Scale)";
                case "decimal":
                    return "decimal(@Prec, @Scale)";
                default:
                    return str.ToLower();
            }
        }
    }
}