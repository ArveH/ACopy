namespace ACommandLineParser.Arguments
{
    public class PasswordArgument: ArgumentBase
    {
        public PasswordArgument()
        {
            Name = "Password";
            ShortName = "-P";
        }

        public override string Syntax => "-P<password>";

        public override string Description => "The password for the login (Sql Server) or user (Oracle)";

        public override bool IsOptional => false;

        protected static bool HasAllConnectDetails(IArgumentCollection args)
        {
            return (args["User"].IsSet && args["Password"].IsSet && args["Server"].IsSet && !args["ConnectionString"].IsSet);
        }

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return HasAllConnectDetails(args);
        }
    }
}
