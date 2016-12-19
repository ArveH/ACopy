namespace AParser
{
    public class AParser: IAParser
    {
        public IASTNodeFactory NodeFactory { get; private set; }

        public AParser(IASTNodeFactory nodeFactory)
        {
            NodeFactory = nodeFactory;
        }

        public bool ExpandEmptyStrings { get; set; }

        public ASTNodeList CreateNodeList(string text)
        {
            ASTNodeList nodes;
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.ExpandEmptyStrings = ExpandEmptyStrings;
            tokenizer.Tokenize(text);
            ATokens tokens = tokenizer.Tokens;

            nodes = ParseExpression(tokens);

            return nodes;
        }

        public ASTNodeList ParseExpression(ATokens tokens)
        {
            ASTNodeList nodes = new ASTNodeList();
            while (tokens.CurrentToken != null)
            {
                IASTNode node = NodeFactory.CreateNode(tokens.CurrentToken.Text, tokens.CurrentToken.AddSpace);
                node.Parse(NodeFactory, this, tokens);
                nodes.Add(node);
            }
            return nodes;
        }

        public IASTNode Accept(ATokens tokens, string text)
        {
            if (tokens.CurrentToken == null)
            {
                throw new AParserException(string.Format("Expected token '{0}' but current token is empty", text));
            }
            if (text != tokens.CurrentToken.Text.ToLower())
            {
                throw new AParserException(string.Format("Expected token '{0}' but found '{1}'", text, tokens.CurrentToken.Text.ToLower()));
            }
            IASTNode node = NodeFactory.CreateNode(tokens.CurrentToken.Text, tokens.CurrentToken.AddSpace);
            node.AddSpace = tokens.CurrentToken.AddSpace;
            tokens.GetNextToken();
            return node;
        }

        public bool Acceptable(ATokens tokens, string text)
        {
            return tokens.CurrentToken != null && tokens.CurrentToken.Text == text;
        }
    }
}
