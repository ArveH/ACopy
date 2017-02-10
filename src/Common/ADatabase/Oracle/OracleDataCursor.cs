using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace ADatabase.Oracle
{
    public class OracleDataCursor: IDataCursor
    {
        private IDbContext _dbContext;
        private InternalOracleConnection _connection;
        private InternalOracleCommand _command;
        private OracleDataReader _reader;

        public OracleDataCursor(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _connection = null;
            _command = null;
            _reader = null;
            RowPreFetch = 5000;
        }

        ~OracleDataCursor()
        {
            Close();
        }

        public int RowPreFetch { get; set; }

        public IDataReader ExecuteReader(string selectStatement, bool hasBlobColumn)
        {
            _connection = new InternalOracleConnection(_dbContext.ConnectionString);

            _command = new InternalOracleCommand(selectStatement, _connection);
            _reader = _command.Command.ExecuteReader();
            if (_command.Command.RowSize > 0)
            {
                _reader.FetchSize = _command.Command.RowSize * RowPreFetch;
            }

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
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
