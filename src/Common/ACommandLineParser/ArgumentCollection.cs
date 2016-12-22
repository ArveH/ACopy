using System;
using System.Collections.Generic;
using System.Linq;
using ACommandLineParser.Exceptions;

namespace ACommandLineParser
{
    public class ArgumentCollection: IArgumentCollection
    {
        private readonly List<IArgument> _arguments;

        public ArgumentCollection()
        {
            _arguments = new List<IArgument>();
        }

        public IArgument this[string key]
        {
            get 
            {
                IArgument arg;
                if (!TryFindArgument(key, out arg))
                {
                    throw new ACommandLineParserException($"Can't find argument: {key}");
                }
                return arg;
            }
        }

        public void Add(IArgument arg)
        {
            _arguments.Add(arg);
        }

        public void Sort()
        {
            _arguments.Sort((a1, a2) => string.Compare(a1.ShortName, a2.ShortName, StringComparison.Ordinal));
        }

        public void AddCommandLineArguments(string[] args)
        {
            foreach (var arg in args.Where(a => a[0] == '-'))
            {
                string value = arg.Substring(2, arg.Length - 2);
                string shortName = arg.Substring(0, 2);
                SetArgumentValue(arg, shortName, value);
            }
        }

        private void SetArgumentValue(string arg, string shortName, string argumentValue)
        {
            IArgument tmpArg;
            if (!TryFindArgument(shortName, out tmpArg))
            {
                throw new ACommandLineParserException($"Illegal command line argument: {arg}");
            }
            tmpArg.Value = argumentValue;
        }

        public bool TryFindArgument(string key, out IArgument argument)
        {
            argument = _arguments.Find(a => a.ShortName == key || a.Name == key);
            return argument != null;
        }

        public bool VerifyArguments()
        {
            return _arguments.TrueForAll(a => a.IsOptional || a.IsRuleOk(this));
        }

        public void Accept(IArgumentVisitor argVisitor)
        {
            _arguments.ForEach(a => a.Accept(argVisitor));
        }
    }
}
