namespace AParser
{
    public class ASTFunctionParameterNode: ASTNodeBase
    {
        public ASTFunctionParameterNode(bool addSpace = false)
        {
            AddSpace = addSpace;
        }

        public override IASTNode Parse(IASTNodeFactory nodeFactory, IAParser aParser, ATokens tokens)
        {
            while (tokens.CurrentToken != null && !aParser.Acceptable(tokens, ASTEndParenthesesNode.KeyWord) && !aParser.Acceptable(tokens, ASTCommaNode.KeyWord))
            {
                IASTNode node = nodeFactory.CreateNode(tokens.CurrentToken.Text, tokens.CurrentToken.AddSpace);
                node.Parse(nodeFactory, aParser, tokens);
                SubNodes.Add(node);
            }
            return this;
        }
    }
}
