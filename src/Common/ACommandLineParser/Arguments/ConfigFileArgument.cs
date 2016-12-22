namespace ACommandLineParser.Arguments
{
    public class ConfigFileArgument: ArgumentBase
    {
        public ConfigFileArgument()
        {
            Name = $"ConfigFile";
            ShortName = "-x";
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

        public override string Syntax => "-x<full_path>";

        public override string Description =>
            "Contains configuration information and settings in an XML format. A description of how to create this file will be given in the documentation when this functionality is done.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return IsSet;
        }
    }
}