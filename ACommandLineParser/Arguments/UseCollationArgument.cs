namespace ACommandLineParser.Arguments
{
    public class UseCollationArgument: ArgumentBase
    {
        public UseCollationArgument()
        {
            Name = "UseCollation";
            ShortName = "-e";
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

        public override string Syntax => "-e[<collation>]";

        public override string Description => "Sql Server only. Override the default database collation. If no collation is given, it will use the collation it finds in the datafile (the collation from the database it was copied from).";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
