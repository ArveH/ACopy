namespace AParser
{
    public class OracleDayAddTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            IASTNode newNode = nodeFactory.CreateNode(ASTStartParenthesesNode.KeyWord, false);
            newNode.SubNodes.Add(nodeFactory.CreateFunctionParameterNode());
            newNode.SubNodes.Add(nodeFactory.CreateNode("+"));
            newNode.SubNodes.Add(nodeFactory.CreateFunctionParameterNode());
            newNode.SubNodes.Add(nodeFactory.CreateNode(ASTEndParenthesesNode.KeyWord));

            newNodes.Add(newNode);
            translator.TranslateSubNodes(newNode.SubNodes[0].SubNodes, subNodes[1]);
            translator.TranslateSubNodes(newNode.SubNodes[2].SubNodes, subNodes[3]);
        }
    }
}
