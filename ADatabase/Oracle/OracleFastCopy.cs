using System;
using System.Data;
using ADatabase.Exceptions;
using Oracle.DataAccess.Client;

namespace ADatabase.Oracle
{
    public class OracleFastCopy: IFastCopy
    {
        private readonly IDbContext _dbContext;

        public OracleFastCopy(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public long LoadData(IDataReader reader, string fileName, ITableDefinition tableDefinition)
        {
            long rowsCopied;
            OracleBulkCopy oracleBulkCopy = null;
            try
            {
                oracleBulkCopy = new OracleBulkCopy(_dbContext.ConnectionString)
                {
                    DestinationTableName = tableDefinition.Name,
                    BatchSize = BatchSize,
                    BulkCopyTimeout = 3*60*60
                };
                // 3 hours
                using (reader)
                {
                    oracleBulkCopy.WriteToServer(reader);
                    rowsCopied = reader.RecordsAffected;
                }
            }
            catch(Exception ex)
            {
                throw new ADatabaseException("OracleBulkCopy threw error", ex);
            }
            finally
            {
                if (oracleBulkCopy != null)
                {
                    oracleBulkCopy.Close();
                    oracleBulkCopy.Dispose();
                }
            }

            return rowsCopied;
        }

        long _largeBlobSize = 100000000;
        public long LargeBlobSize
        {
            get { return _largeBlobSize; }
            set { _largeBlobSize = value; }
        }

        int _batchSize = 10000;
        public int BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }
    }
}
