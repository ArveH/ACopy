using ADatabase;

namespace ACopyLib.Xml
{
    public static class AXmlFactory
    {
        public static IAXmlWriter CreateWriter()
        {
            return new AXmlWriter();
        }

        public static IAXmlReader CreateReader(IDbContext dbContext)
        {
            return new AXmlReader(dbContext);
        }
    }
}