namespace ACommandLineParser.Arguments
{
    public class ViewArgument: ArgumentBase
    {
        public ViewArgument()
        {
            Name = "View";
            ShortName = "-v";
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

        public override string Syntax => "-v";

        public override string Description => "The views found in asysview and aagview will be created. If the same view exists in both tables, the one in aagview will be created.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
