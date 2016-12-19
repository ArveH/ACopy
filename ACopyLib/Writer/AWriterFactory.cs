using ADatabase;
using ALogger;

namespace ACopyLib.Writer
{
    public static class AWriterFactory
    {
        public static IAWriter CreateInstance(IDbContext dbContext, IALogger logger=null)
        {
            if (logger == null)
            {
                return new AWriter(dbContext, new TestLogger());
            }

            return new AWriter(dbContext, logger);
        }
    }
}