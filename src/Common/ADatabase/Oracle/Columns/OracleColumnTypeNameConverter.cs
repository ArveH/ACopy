using ADatabase.Exceptions;

namespace ADatabase.Oracle.Columns
{
    public static class OracleColumnTypeNameConverter
    {
        public static ColumnTypeName Native2ACopy(string nativeType)
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

            throw new ADatabaseException($"Copy program doesn't have a representation for column type {nativeType}");
        }

    }
}