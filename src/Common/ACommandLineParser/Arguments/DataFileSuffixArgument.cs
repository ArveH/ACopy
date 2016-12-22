namespace ACommandLineParser.Arguments
{
    public class DataFileSuffixArgument: ArgumentBase
    {
        public DataFileSuffixArgument()
        {
            Name = "DataFileSuffix";
            ShortName = "-g";
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

        public override string Syntax => "-g<datafile_suffix>";

        public override string Description => 
            "Default suffix for a datafile is 'adata', but you can set it to what you want. " +
            "OBS: Remember to use the same value for copy in, as copy out";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return IsSet;
        }

    }
}