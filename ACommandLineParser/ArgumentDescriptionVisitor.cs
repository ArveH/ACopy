using System.Text;

namespace ACommandLineParser
{
    public class ArgumentDescriptionVisitor : IArgumentVisitor
    {
        private readonly StringBuilder _usageString;

        public ArgumentDescriptionVisitor()
        {
            _usageString = new StringBuilder();
        }

        public void VisitArgument(IArgument arg)
        {
            if (arg.IsOptional)
            {
                _usageString.Append("[" + arg.Syntax + "] ");
            }
            else
            {
                _usageString.Append(arg.Syntax + " ");
            }
            _usageString.AppendLine();
            
            _usageString.Append(arg.Description);
            _usageString.AppendLine();
            _usageString.AppendLine();
        }

        public string GetUsage(string program)
        {
            _usageString.Insert(0, "Usage: " + program + "\n\n");
            return _usageString.ToString();
        }
    }
}
