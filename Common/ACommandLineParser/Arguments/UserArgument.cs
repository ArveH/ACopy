namespace ACommandLineParser.Arguments
{
    public class UserArgument: ArgumentBase
    {
        public UserArgument()
        {
            Name = "User";
            ShortName = "-U";
        }
        public override string Syntax => "-U<user_name>";

        public override string Description => "The name of the the login (Sql Server) or user (Oracle)";

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
