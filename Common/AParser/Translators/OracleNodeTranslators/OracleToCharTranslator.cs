namespace AParser
{
    public class OracleToCharTranslator : ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "to_char", CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
        }
    }
}
