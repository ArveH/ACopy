using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ACommandLineParser.Arguments;

namespace ACommandLineParser
{
    public static class ArgumentCollectionFactory
    {
        public static IArgumentCollection CreateArgumentCollection()
        {
            var argumentNames = ReflectAllArguments();

            IArgumentCollection arguments = new ArgumentCollection();
            foreach (var arg in argumentNames)
            {
                var argument = ArgumentFactory.CreateArgument(arg.Substring(0, arg.Length-"Argument".Length)); // Remove "Argument" from class name. E.g. "DirectionArgument" -> "Direction"
                arguments.Add(argument);
            }
            arguments.Sort();
            return arguments;
        }

        private static List<string> ReflectAllArguments()
        {
            Assembly assembly = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                where a.GetName().Name == "ACommandLineParser"
                                select a).First();
            return (from c in assembly.GetTypes()
                    where c.IsSubclassOf(typeof(ArgumentBase))
                    select c.Name).ToList();
        }
    }
}
