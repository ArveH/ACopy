namespace ACommandLineParser.Arguments
{
    public class ConnectionStringArgument: ArgumentBase
    {
        public ConnectionStringArgument()
        {
            Name = "ConnectionString";
            ShortName = "-C";
        }

        public override string Syntax => "-C<connection_string_name>";

        public override string Description => "A connection string name that exists in the acopy.xml config file. OBS: Not the connection string itself";

        public override bool IsOptional => true;

        protected static bool ConnectDetailsAreNotUsed(IArgumentCollection args)
        {
            return !args["User"].IsSet && !args["Password"].IsSet && !args["Server"].IsSet;
        }

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return ConnectDetailsAreNotUsed(args);
        }
    }
}
