using System.Collections.Generic;

namespace AParser
{
    public interface IASTNodeFactory
    {
        Dictionary<string, IASTNode> KeyTokens { get; }
        IASTNode CreateNode(string text, bool addSpace = true);
        IASTNode CreateFunctionParameterNode(bool addSpace = false);
    }
}
