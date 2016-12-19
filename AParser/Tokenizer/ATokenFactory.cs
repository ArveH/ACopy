namespace AParser
{
    public class ATokenFactory: IATokenFactory
    {
        public IAToken CreateToken(string text, bool addSpace=true)
        {
            return new AToken(text, addSpace);
        }
    }
}
