using System;
using System.Globalization;

namespace ADatabase.Oracle.Columns
{
    public class OracleTimestampColumn: OracleDateColumn
    {
        private readonly string _typeToString;

        public OracleTimestampColumn(string name, int length, bool isNullable, string def)
            : base(name, isNullable, ConvertNativeDateToKeyword(def))
        {
            if (length > 0)
            {
                Type = ColumnTypeName.Timestamp;
                Details["Length"] = length;
                _typeToString = $"timestamp({length})";
            }
            else
            {
                _typeToString = "timestamp";
            }
        }


        public override string TypeToString()
        {
            return _typeToString;
        }

    }
}