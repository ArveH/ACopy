﻿using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleCharColumn : OracleColumn
    {
        public OracleCharColumn(string name, int length, bool isNullable, string def)
            : base(name, ColumnTypeName.Char, isNullable, def)
        {
            Details["Length"] = length;
        }

        public override string TypeToString()
        {
            return $"char({(int) Details["Length"]} char)";
        }

        public override string ToString(object value)
        {
            return "'" + Convert.ToString(value).Replace("'", "''").TrimEnd(' ') + "'";
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return string.IsNullOrEmpty(value) ? " " : value;
        }

        public override Type GetDotNetType()
        {
            return typeof(string);
        }
    }
}