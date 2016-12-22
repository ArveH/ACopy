namespace AParser
{
    public class SqlServerModTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // MOD(3, 5) => (3) % (5)
            Translator = translator;
            NodeFactory = nodeFactory;

            WrapParameterInParentheses(newNodes, subNodes[1]);
            newNodes.Add(nodeFactory.CreateNode("%"));
            WrapParameterInParentheses(newNodes, subNodes[3]);
        }

        private void WrapParameterInParentheses(ASTNodeList newNodes, IASTNode parameter)
        {
            IASTNode newNode = NodeFactory.CreateNode(ASTStartParenthesesNode.KeyWord, false);
            Translator.TranslateSubNodes(newNode.SubNodes, parameter);
            newNode.SubNodes.Add(NodeFactory.CreateNode(ASTEndParenthesesNode.KeyWord));
            newNodes.Add(newNode);
        }
    }
}
