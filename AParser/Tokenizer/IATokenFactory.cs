namespace AParser
{
    public interface IATokenFactory
    {
        IAToken CreateToken(string text, bool addSpace = true);
    }
}
