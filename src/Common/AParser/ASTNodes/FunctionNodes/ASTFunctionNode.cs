namespace AParser
{
    public abstract class ASTFunctionNode: ASTNodeBase
    {
        public ASTFunctionNode(bool addSpace = true)
        {
            AddSpace = addSpace;
        }

        public override IASTNode Parse(IASTNodeFactory nodeFactory, IAParser aParser, ATokens tokens)
        {
            aParser.Accept(tokens, Text);
            ParseParameters(nodeFactory, aParser, tokens);
            return this;
        }

        protected void ParseParameters(IASTNodeFactory nodeFactory, IAParser aParser, ATokens tokens)
        {
            SubNodes.Add(aParser.Accept(tokens, ASTStartParenthesesNode.KeyWord));

            while (tokens.CurrentToken != null && !aParser.Acceptable(tokens, ASTEndParenthesesNode.KeyWord))
            {
                IASTNode node = nodeFactory.CreateFunctionParameterNode(tokens.CurrentToken.AddSpace);
                node.Parse(nodeFactory, aParser, tokens);
                SubNodes.Add(node);

                if (aParser.Acceptable(tokens, ASTCommaNode.KeyWord))
                {
                    SubNodes.Add(aParser.Accept(tokens, ASTCommaNode.KeyWord));
                }
            }

            SubNodes.Add(aParser.Accept(tokens, ASTEndParenthesesNode.KeyWord));
        }
    }
}
