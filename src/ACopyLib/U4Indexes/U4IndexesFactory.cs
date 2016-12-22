using ADatabase;

namespace ACopyLib.U4Indexes
{
    public static class U4IndexesFactory
    {
        public static IU4Indexes CreateInstance(IDbContext dbContext)
        {
            return new U4Indexes(dbContext);
        }
    }
}
