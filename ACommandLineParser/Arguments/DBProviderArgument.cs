namespace ACommandLineParser.Arguments
{
    public class DbProviderArgument: ArgumentBase
    {
        public DbProviderArgument()
        {
            Name = "DBProvider";
            ShortName = "-R";
        }

        public override string Syntax => "-R{O|M}";

        public override string Description => "The database provider you are running against, Oracle (O) or Microsoft SQL Server (M)";

        public override bool IsOptional => false;

        protected override bool IsInternalRuleOk(IArgumentCollection args)
        {
            return IsSet;
        }
    }
}
