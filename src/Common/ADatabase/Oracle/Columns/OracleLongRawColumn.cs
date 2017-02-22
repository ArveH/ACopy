using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleLongRawColumn: OracleColumn
    {
        public OracleLongRawColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.OldRaw, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "long raw";
        }

        public override string Default
        {
            get
            {
                return base.Default;
            }
            set
            {
                base.Default = value;
                if (base.Default.IndexOf("cast_to_raw", StringComparison.Ordinal) >= 0)
                {
                    base.Default = "";
                }
            }
        }

        public override string ToString(object value)
        {
            throw new NotImplementedException("Column.ToFile for OLDRAW");
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException("Column.ToInternalType for OLDRAW");
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}