using System;
using System.Text;

namespace ADatabase.Oracle.Columns
{
    public class OracleRawColumn: OracleColumn
    {
        public OracleRawColumn(string name, int length, bool isNullable, string def) 
            : base(name, ColumnTypeName.Raw, isNullable, def)
        {
            Details["Length"] = length;
            if (length == 16)
            {
                Type = ColumnTypeName.Guid;
                base.Default = ConvertNativeFunctionToKeyword(def);
            }
        }

        public override string TypeToString()
        {
            // Workaround to get OracleDataReader to work with raw(16):
            //    create column as raw(17)
            //    copy data
            //    resize to raw(16)
            return $"raw({Details["Length"]})";
        }

        protected override string ParseDefaultValue(string def)
        {
            switch (def)
            {
                case "GUID":
                    return "sys_guid()";
            }

            return def;
        }

        public override string Default
        {
            get { return base.Default; }
            set
            {
                if (IsGuid()) base.Default = ConvertNativeFunctionToKeyword(value);
                else base.Default = value;
            }
        }

        private bool IsGuid()
        {
            return Type == ColumnTypeName.Guid;
        }

        private static string ConvertNativeFunctionToKeyword(string guid)
        {
            if (guid.IndexOf("sys_guid", StringComparison.Ordinal) >= 0)
            {
                return "GUID";
            }

            return guid;
        }

        public override string ToString(object value)
        {
            if (IsGuid()) return OracleGuidHelper.ConvertToGuid((byte[])value).ToString();
            return Encoding.UTF8.GetString((byte[])value);
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            if (IsGuid()) return OracleGuidHelper.ConvertToByteArray(Guid.Parse(value));
            return Encoding.UTF8.GetBytes(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }

    }
}