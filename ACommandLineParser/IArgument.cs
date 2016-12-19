namespace ACommandLineParser
{
    public interface IArgument
    {
        string Name { get; }
        string ShortName { get; }
        string Value { get; set; }
        string Syntax { get; }
        string Description { get; }
        bool IsSet { get; }
        bool IsOptional { get; }
        bool IsRuleOk(IArgumentCollection arguments);
        void Accept(IArgumentVisitor visitor);
    }
}
