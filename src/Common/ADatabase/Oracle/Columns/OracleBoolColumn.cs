using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleBoolColumn : OracleColumn
    {
        public OracleBoolColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Bool, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return string.Format("number(1)");
        }

        public override string ToString(object value)
        {
            return Convert.ToInt16(value).ToString();
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value.Length == 1 ? value == "1" : Convert.ToBoolean(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(bool);
        }
    }
}