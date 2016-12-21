using System;
using ADatabase.Exceptions;

namespace ADatabase.Oracle
{
    public class OracleCommands: ICommands
    {
        private IDbContext _dbContext;
        public OracleCommands(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int ExecuteNonQuery(string sql)
        {
            InternalOracleConnection connection = null;
            try
            {
                connection = new InternalOracleConnection(_dbContext.ConnectionString);
	            using (var command = new InternalOracleCommand(sql, connection))
	            {
	                return command.Command.ExecuteNonQuery();
	            }
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR with statement: " + sql, ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public object ExecuteScalar(string sql)
        {
            InternalOracleConnection connection = null;
            try
            {
                connection = new InternalOracleConnection(_dbContext.ConnectionString);
                using (InternalOracleCommand command = new InternalOracleCommand(sql, connection))
                {
                    return command.Command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR with statement: " + sql, ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
    }
}
