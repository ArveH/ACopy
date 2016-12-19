using System.Collections.Generic;
using System.Text;

namespace AParser
{
    public class ASTNodeList: List<IASTNode>
    {
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            foreach (var node in this)
            {
                text.Append(node.ToString());
            }

            return text.ToString();
        }
    }
}
