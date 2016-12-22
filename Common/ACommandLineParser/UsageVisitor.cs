using System.Text;

namespace ACommandLineParser
{
    public class UsageVisitor: IArgumentVisitor
    {
        private readonly StringBuilder _usageString;

        public UsageVisitor()
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
                _usageString.Append(" " + arg.Syntax + " ");
            }
            if (!arg.IsOptional && !arg.IsSet)
            {
                _usageString.AppendFormat("{0}", "(MISSING)".PadLeft(30-arg.Syntax.Length));
            }
            _usageString.AppendLine();
        }

        public string GetUsage(string program)
        {
            _usageString.Insert(0, "Usage: " + program + "\n");
            return _usageString.ToString();
        }
    }
}
