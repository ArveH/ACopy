namespace ACommandLineParser.Arguments
{
    public class ServerArgument: ArgumentBase
    {
        public ServerArgument()
        {
            Name = "Server";
            ShortName = "-S";
        }

        public override string Syntax => "-S<server_name>";

        public override string Description => "The name of the database server. E.g. myserver\\instance for SQL Server, or myserver/orcl for Oracle. OBS: ODBC connection is not allowed.";

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
