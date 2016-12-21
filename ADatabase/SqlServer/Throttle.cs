using System;
using System.Threading;
using ADatabase.Exceptions;

namespace ADatabase.SqlServer
{
    public static class Throttle
    {
        private static readonly int _maxTries = 30;

        public static T Execute<T>(IDbContext dbContext, string sql, Func<InternalSqlServerCommand, T> func)
        {
            for (int i = 0; i < _maxTries; i++)
            {
                try
                {
                    return InternalSqlServerConnection.ExecuteInConnectionScope(dbContext, sql, func);
                }
                catch (Exception ex)
                {
                    if (ADatabaseException.ShouldThrottle(ex))
                    {
                        dbContext.Logger.Write(String.Format("Throttling down \"{0}...\", round {1}", sql.Substring(0, Math.Min(sql.Length, 40)), i));
                        Thread.Sleep(10000 + i * i * i * 1000);
                    }
                    else
                        throw;
                }
            }

            return InternalSqlServerConnection.ExecuteInConnectionScope(dbContext, sql, func);
        }
    }
}
