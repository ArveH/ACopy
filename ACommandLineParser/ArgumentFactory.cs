using ACommandLineParser.Arguments;
using ACommandLineParser.Exceptions;

namespace ACommandLineParser
{
    public static class ArgumentFactory
    {
        public static IArgument CreateArgument(string argumentName)
        {
            switch (argumentName)
            {
                case "ArgumentDescription":
                    return new ArgumentDescriptionArgument();
                case "BatchSize":
                    return new BatchSizeArgument();
                case "CreateClusteredIndex":
                    return new CreateClusteredIndexArgument();
                case "ConnectionString":
                    return new ConnectionStringArgument();
                case "Database":
                    return new DatabaseArgument();
                case "DataFileSuffix":
                    return new DataFileSuffixArgument();
                case "DbProvider":
                    return new DbProviderArgument();
                case "Direction":
                    return new DirectionArgument();
                case "Folder":
                    return new FolderArgument();
                case "MaxDegreeOfParallelism":
                    return new MaxDegreeOfParallelismArgument();
                case "Password":
                    return new PasswordArgument();
                case "Server":
                    return new ServerArgument();
                case "SchemaFileSuffix":
                    return new SchemaFileSuffixArgument();
                case "UseCollation":
                    return new UseCollationArgument();
                case "UseCompression":
                    return new UseCompressionArgument();
                case "User":
                    return new UserArgument();
                case "View":
                    return new ViewArgument();
            }

            throw new ACommandLineParserException($"Can't create argument: {argumentName}");
        }
    }
}
