namespace ACommandLineParser.Arguments
{
    public class ArgumentDescriptionArgument: ArgumentBase
    {
        public ArgumentDescriptionArgument()
        {
            Name = "ArgumentDescription";
            ShortName = "-?";
            _isSet = false;
        }

        private bool _isSet;
        public override bool IsSet => _isSet;

        public override string Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value;
                _isSet = true;
            }
        }

        public override string Syntax => "-?";

        public override string Description => "Display description of all command line arguments.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
