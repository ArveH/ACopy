namespace ACommandLineParser.Arguments
{
    public class ConversionsFileArgument: ArgumentBase
    {
        public ConversionsFileArgument()
        {
            Name = $"ConversionsFile";
            ShortName = "-k";
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

        public override string Syntax => "-k<file_name>";

        public override string Description =>
            "An xml file that contains rules for how column types are handled. This is used when you want to copy " +
            "tables between RDBMS's, or you want to convert the column type of some (or all) columns. ";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return IsSet;
        }
    }
}