namespace ACommandLineParser.Arguments
{
    public class CreateClusteredIndexArgument: ArgumentBase
    {
        public CreateClusteredIndexArgument()
        {
            Name = "CreateClusteredIndex";
            ShortName = "-i";
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

        public override string Syntax => "-i";

        public override string Description => "A clustered index is created on the agrtid column.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
