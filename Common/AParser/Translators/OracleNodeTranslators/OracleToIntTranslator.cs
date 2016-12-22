namespace AParser
{
    public class OracleToIntTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "to_number", CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
        }
    }
}
