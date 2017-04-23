namespace ACommandLineParser
{
    public interface IArgumentCollection
    {
        IArgument this[string key] { get; }
        void Add(IArgument arg);
        void AddCommandLineArguments(string[] args);
        void Sort();
        bool VerifyArguments();
        void Accept(IArgumentVisitor argVisitor);
    }
}
