namespace ACommandLineParser
{
    public interface IArgumentCollection
    {
        IArgument this[string key] { get; }
        void Add(IArgument arg);
        void AddCommandLineArguments(string[] args);
        void Sort();
        bool TryFindArgument(string key, out IArgument argument);
        bool VerifyArguments();
        void Accept(IArgumentVisitor argVisitor);
    }
}
