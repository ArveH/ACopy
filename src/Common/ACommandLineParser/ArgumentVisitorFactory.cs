namespace ACommandLineParser
{
    public static class ArgumentVisitorFactory
    {
        public static IArgumentVisitor CreateArgumentVisitor(bool fullDescription = false)
        {
            if (fullDescription)
            {
                return new ArgumentDescriptionVisitor();
            }
            return new UsageVisitor();
        }
    }
}
