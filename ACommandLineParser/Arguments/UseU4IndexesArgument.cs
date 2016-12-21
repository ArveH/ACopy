namespace ACommandLineParser.Arguments
{
    public class UseU4IndexesArgument: ArgumentBase
    {
        public UseU4IndexesArgument()
        {
            Name = "UseU4Indexes";
            ShortName = "-j";
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

        public override string Syntax => "-j";

        public override string Description => "Using this parameter, the copy program will not look at the actual indexes on the table, but will create indexes according to values in aag/asysindex tables.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }

    }
}