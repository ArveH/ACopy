using ADatabase;
using ALogger;

namespace ACopyLib.Reader
{
    public static class AReaderFactory
    {
        public static IAReader CreateInstance(IDbContext dbContext, IALogger logger=null)
        {
            if (logger == null)
            {
                return new AReader(dbContext, new TestLogger());
            }

            return new AReader(dbContext, logger);
        }
    }
}