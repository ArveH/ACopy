namespace ACommandLineParser.Arguments
{
    public abstract class ArgumentBase: IArgument
    {
        public virtual string Value { get; set; }

        public virtual bool IsSet => !string.IsNullOrEmpty(Value);

        public string Name { get; protected set; }

        public string ShortName { get; protected set; }

        public abstract string Syntax { get; }
        public abstract string Description { get; }
        public abstract bool IsOptional { get; }
        protected abstract bool IsInternalRuleOk(IArgumentCollection args);

        public bool IsRuleOk(IArgumentCollection args)
        {
            return (IsSet == false && IsOptional) || IsInternalRuleOk(args);
        }

        public void Accept(IArgumentVisitor visitor)
        {
            visitor.VisitArgument(this);
        }
    }
}
