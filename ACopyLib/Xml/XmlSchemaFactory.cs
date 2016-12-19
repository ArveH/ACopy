using ADatabase;

namespace ACopyLib.Xml
{
    public static class XmlSchemaFactory
    {
        public static IXmlSchema CreateInstance(IDbContext dbContext)
        {
            return new XmlSchema(dbContext);
        }
    }
}
