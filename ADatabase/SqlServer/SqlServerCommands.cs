namespace ADatabase.SqlServer
{
    public class SqlServerCommands : ICommands
    {
        private IDbContext _dbContext;

        public SqlServerCommands(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int ExecuteNonQuery(string sql)
        {
            return Throttle.Execute(
                _dbContext, 
                sql,
                cmd => cmd.Command.ExecuteNonQuery()
                );

        }

        public object ExecuteScalar(string sql)
        {
            return Throttle.Execute(
                _dbContext, 
                sql, 
                cmd => cmd.Command.ExecuteScalar()
                );
        }
    }
}