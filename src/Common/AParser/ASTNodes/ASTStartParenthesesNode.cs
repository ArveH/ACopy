namespace AParser
{
    public class ASTStartParenthesesNode: ASTNodeBase
    {
        public ASTStartParenthesesNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "("; } }

        public override IASTNode Parse(IASTNodeFactory nodeFactory, IAParser aParser, ATokens tokens)
        {
            tokens.GetNextToken();
            while (tokens.CurrentToken != null && !aParser.Acceptable(tokens, ASTEndParenthesesNode.KeyWord))
            {
                IASTNode newNode = aParser.NodeFactory.CreateNode(tokens.CurrentToken.Text, tokens.CurrentToken.AddSpace);
                newNode.Parse(aParser.NodeFactory, aParser, tokens);
                SubNodes.Add(newNode);
            }
            if (!aParser.Acceptable(tokens, ASTEndParenthesesNode.KeyWord))
            {
                throw new AParserException("Didn't find closing parentheses");
            }
            SubNodes.Add(aParser.Accept(tokens, ASTEndParenthesesNode.KeyWord));
            return this;
        }
    }
}
