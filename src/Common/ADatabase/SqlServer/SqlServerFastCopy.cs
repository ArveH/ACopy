using System.Data;
using System.Data.SqlClient;

namespace ADatabase.SqlServer
{
    public class SqlServerFastCopy : IFastCopy
    {
        private readonly IDbContext _dbContext;

        public SqlServerFastCopy(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public long LoadData(IDataReader reader, string fileName, ITableDefinition tableDefinition)
        {
            long rowsCopied;
            using (var sqlBulkCopy = new SqlBulkCopy(_dbContext.ConnectionString, SqlBulkCopyOptions.TableLock))
            {
                sqlBulkCopy.DestinationTableName = tableDefinition.Name;
                sqlBulkCopy.EnableStreaming = true;
                sqlBulkCopy.BulkCopyTimeout = 0;
                sqlBulkCopy.BatchSize = BatchSize;

                using (reader)
                {
                    sqlBulkCopy.WriteToServer(reader);
                    rowsCopied = reader.RecordsAffected;
                }
            }

            return rowsCopied;
        }

        private long _largeBlobSize = 100000000;
        public long LargeBlobSize
        {
            get { return _largeBlobSize; }
            set { _largeBlobSize = value; }
        }

        private int _batchSize = 1000;
        public int BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }
    }
}