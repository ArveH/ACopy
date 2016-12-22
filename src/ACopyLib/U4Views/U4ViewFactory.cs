using ADatabase;

namespace ACopyLib.U4Views
{
    public static class U4ViewFactory
    {
        public static IU4Views CreateInstance(IDbContext dbContext)
        {
            return new U4Views(dbContext); 
        }
    }
}
