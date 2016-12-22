namespace AParser
{
    public static class AParserFactory
    {
        public static IAParser CreateInstance(IASTNodeFactory nodeFactory)
        {
            return new AParser(nodeFactory);
        }
    }
}
