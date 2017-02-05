using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleBlobColumn: OracleColumn
    {
        public OracleBlobColumn(string name, bool isNullable, string def)
            : base(name, ColumnTypeName.Blob, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "blob";
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
                if (base.Default.IndexOf("empty_blob", StringComparison.Ordinal) >= 0)
                {
                    base.Default = "";
                }
            }
        }

        public override string ToString(object value)
        {
            throw new NotImplementedException("Column.ToFile for BLOB");
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException("Column.ToInternalType for BLOB");
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}