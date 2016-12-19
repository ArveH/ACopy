namespace ACommandLineParser.Arguments
{
    public class DatabaseArgument: ArgumentBase
    {
        public DatabaseArgument()
        {
            Name = "Database";
            ShortName = "-D";
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

        public override string Syntax => "-D";

        public override string Description => "SQL Server only. If you want to connect to another database then the default for your Login, you have to use this parameter.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
