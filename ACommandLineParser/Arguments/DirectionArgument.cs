using ACommandLineParser.Exceptions;

namespace ACommandLineParser.Arguments
{
    public class DirectionArgument: ArgumentBase
    {
        public DirectionArgument()
        {
            Name = "Direction";
            ShortName = "-d";
        }

        public override string Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value.ToLower();
                if (base.Value != "in" && base.Value != "out")
                {
                    throw new ACommandLineParserException($"ERROR: '{base.Value}' is not a legal direction value");
                }
            }
        }
        public override string Syntax => "-d{in|out}";

        public override string Description => "The direction of the data flow. (in)to the database, or (out) from the database";

        public override bool IsOptional => false;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return IsSet;
        }
    }
}
