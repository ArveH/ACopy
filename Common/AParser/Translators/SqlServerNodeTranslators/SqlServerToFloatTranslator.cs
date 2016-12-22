namespace AParser
{
    public class SqlServerToFloatTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // TO_FLOAT('123') => convert(dec(28,8) , '123')
            Translator = translator;
            NodeFactory = nodeFactory;

            IASTNode newNode = NodeFactory.CreateNode("convert", false);
            newNode.SubNodes.Add(NodeFactory.CreateNode(ASTStartParenthesesNode.KeyWord, false));
            newNode.SubNodes.Add(NodeFactory.CreateFunctionParameterNode());
            newNode.SubNodes.Add(NodeFactory.CreateNode(ASTCommaNode.KeyWord));
            newNode.SubNodes.Add(CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
            newNode.SubNodes.Add(NodeFactory.CreateNode(ASTEndParenthesesNode.KeyWord));

            AddFunction(newNode.SubNodes[1].SubNodes, "dec", CreateFunctionParameterNode("28"), CreateFunctionParameterNode("8"));
            newNodes.Add(newNode);
        }
    }
}
