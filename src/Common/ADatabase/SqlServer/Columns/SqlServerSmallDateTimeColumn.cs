namespace ADatabase.SqlServer.Columns
{
    public class SqlServerSmallDateTimeColumn: SqlServerDatetimeColumn
    {
        public SqlServerSmallDateTimeColumn(string name, bool isNullable, string def)
            : base(name, isNullable, ConvertNativeDateToKeyword(def))
        {
            Type = ColumnTypeName.SmallDateTime;
        }

        public override string TypeToString()
        {
            return "smalldatetime";
        }

        protected override string ParseDefaultValue(string def)
        {
            switch (def)
            {
                case "MIN_DATE":
                    return "convert(smalldatetime,'19000101 00:00:00:000',9)";
                case "MAX_DATE":
                    return "convert(smalldatetime,'20991231 23:59:59:998',9)";
                case "TS2DAY(MAX_DATE)":
                    return "convert(smalldatetime,'20991231 00:00:00:000',9)";
                case "TODAY":
                    return "CAST( FLOOR( CAST( getdate() AS FLOAT ) )AS SMALLDATETIME)";
                case "NOW":
                    return "getdate()";
            }

            return def;
        }
    }
}