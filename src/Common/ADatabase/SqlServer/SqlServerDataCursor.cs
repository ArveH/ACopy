using System.Data;

namespace ADatabase.SqlServer
{
    public class SqlServerDataCursor: IDataCursor
    {
        private IDbContext _dbContext;
        private InternalSqlServerConnection _connection;
        private InternalSqlServerCommand _command;
        private IDataReader _reader;

        public SqlServerDataCursor(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _connection = null;
            _command = null;
            _reader = null;
        }

        ~SqlServerDataCursor()
        {
            Close();
        }

        public IDataReader ExecuteReader(string selectStatement, bool hasBlobColumn)
        {
            _connection = new InternalSqlServerConnection(_dbContext.ConnectionString);
            _command = new InternalSqlServerCommand(selectStatement, _connection);
            CommandBehavior behavior = CommandBehavior.Default;
            if (hasBlobColumn)
            {
                behavior |= CommandBehavior.SequentialAccess;
            }
            _reader = _command.Command.ExecuteReader(behavior);

            return _reader;
        }

        public void Close()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
