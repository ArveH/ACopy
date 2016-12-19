namespace ACommandLineParser.Arguments
{
    public class UseCompressionArgument: ArgumentBase
    {
        public UseCompressionArgument()
        {
            Name = "UseCompression";
            ShortName = "-c";
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

        public override string Syntax => "-c";

        public override string Description => "The data files will be compressed using the Deflate algorithm.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return true;
        }
    }
}
