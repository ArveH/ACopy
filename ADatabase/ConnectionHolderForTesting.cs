namespace ADatabase
{
    public static class ConnectionHolderForTesting
    {
        public static string GetOracleConnection()
        {
            return "ora_ah09";
        }

        public static string GetSqlServerConnection()
        {
            return "mss_local";
        }
    }
}
