namespace ACommandLineParser.Arguments
{
    public class MaxDegreeOfParallelismArgument: ArgumentBase
    {
        public MaxDegreeOfParallelismArgument()
        {
            Name = "MaxDegreeOfParallelism";
            ShortName = "-m";
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

        public override string Syntax => "-m";

        public override string Description => "Limit the number of tables that will be read/written in parallel. Default is -1, which means there is no limit. You might want to set a limit when your data source is an SQL Azure database.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
