using ACommandLineParser.Exceptions;

namespace ACommandLineParser.Arguments
{
    public class BatchSizeArgument: ArgumentBase
    {
        public BatchSizeArgument()
        {
            Name = "BatchSize";
            ShortName = "-b";
            _isSet = false;
        }

        private bool _isSet;
        public override bool IsSet => _isSet;

        public override string Value
        {
            get { return base.Value; }
            set
            {
                int dummy;
                if (!int.TryParse(value, out dummy))
                {
                    throw new ACommandLineParserException($"ERROR: Illegal batch size ({value}) for -b");
                }
                base.Value = value;
                _isSet = true;
            }
        }

        public override string Syntax => "-b<batch_size>";

        public override string Description => "For copy in only (-din). Collects rows in a \"batch\", and sends the batch to the server as one operation. \nSQL Server: Default is 0 (zero), which means that we send for every row.\nOracle: Default is 10.000.";

        public override bool IsOptional => true;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return IsSet;
        }
    }
}
