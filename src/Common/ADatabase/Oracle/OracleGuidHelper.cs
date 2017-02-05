using System;

namespace ADatabase.Oracle
{
    public static class OracleGuidHelper
    {
        public static Guid ConvertToGuid(byte[] b)
        {
            if (b.Length == 16)
            {
                Swap(ref b[0], ref b[3]);
                Swap(ref b[1], ref b[2]);
                Swap(ref b[4], ref b[5]);
                Swap(ref b[6], ref b[7]);
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
    }
}