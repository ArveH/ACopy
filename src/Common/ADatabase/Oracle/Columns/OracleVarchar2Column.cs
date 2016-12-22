using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleVarchar2Column: OracleColumn
    {
        public OracleVarchar2Column(string name, int length, bool isNullable, string def)
            : base(name, ColumnType.Varchar, isNullable, def)
        {
            Details["Length"] = length;
        }

        public override string TypeToString()
        {
            return string.Format("varchar2({0} char)", (int)Details["Length"]);
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
