namespace AParser
{
    public static class ATokenizerFactory
    {
        public static IATokenizer CreateInstance()
        {
            return new ATokenizer();
        }
    }
}
