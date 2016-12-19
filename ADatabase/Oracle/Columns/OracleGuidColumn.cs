using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleGuidColumn : OracleColumn
    {
        public OracleGuidColumn(string name, int length, bool isNullable, string def)
            : base(name, ColumnType.Guid, isNullable, ConvertNativeFunctionToKeyword(def))
        {
            Details["length"] = length;
        }


        public override string TypeToString()
        {
            // Workaround to get OracleDataReader to work with raw(16):
            //    create column as raw(17)
            //    copy data
            //    resize to raw(16)
            return string.Format("raw({0})", Details["length"]);
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
                base.Default = ConvertNativeFunctionToKeyword(value);
            }
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
            return ConvertToGuid((byte[])value).ToString();
        }

        public override object ToInternalType(string value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return ConvertToByteArray(Guid.Parse(value));
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }

        #region Guid helper methods
        public static Guid ConvertToGuid(byte[] b)
        {
            if (b.Length == 16)
            {
                Swap(ref b[0], ref b[3]);
                Swap(ref b[1], ref b[2]);
                Swap(ref b[4], ref b[5]);
                Swap(ref b[6], ref  b[7]);
            }

            return new Guid(b);
        }

        public static byte[] ConvertToByteArray(Guid x)
        {
            byte[] b = x.ToByteArray();

            Swap(ref b[0], ref b[3]);
            Swap(ref b[1], ref b[2]);
            Swap(ref b[4], ref b[5]);
            Swap(ref b[6], ref b[7]);
            return b;
        }

        private static void Swap(ref byte a, ref byte b)
        {
            var c = a;
            a = b;
            b = c;
        }
        #endregion

    }
}