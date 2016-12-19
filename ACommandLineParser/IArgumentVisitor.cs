namespace ACommandLineParser
{
    public interface IArgumentVisitor
    {
        string GetUsage(string program);
        void VisitArgument(IArgument arg);
    }
}
