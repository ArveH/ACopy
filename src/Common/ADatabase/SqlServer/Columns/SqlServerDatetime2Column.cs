namespace ADatabase.SqlServer.Columns
{
    public class SqlServerDatetime2Column: SqlServerDatetimeColumn
    {
        public SqlServerDatetime2Column(string name, bool isNullable, string def)
            : base(name, isNullable, ConvertNativeDateToKeyword(def))
        {
            Type = ColumnTypeName.DateTime2;
        }

        public override string TypeToString()
        {
            return "datetime2";
        }

        protected override string ParseDefaultValue(string def)
        {
            switch (def)
            {
                case "MIN_DATE":
                    return "convert(datetime2,'19000101 00:00:00:000',9)";
                case "MAX_DATE":
                    return "convert(datetime2,'20991231 23:59:59:998',9)";
                case "TS2DAY(MAX_DATE)":
                    return "convert(datetime2,'20991231 00:00:00:000',9)";
                case "TODAY":
                    return "CAST( FLOOR( CAST( getdate() AS FLOAT ) )AS DATETIME2)";
                case "NOW":
                    return "getdate()";
            }

            return def;
        }
    }
}