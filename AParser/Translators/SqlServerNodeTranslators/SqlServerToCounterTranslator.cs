namespace AParser
{
    public class SqlServerToCounterTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // TO_COUNTER('9') => convert(bigint, '0') )
            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "convert", CreateFunctionParameterNode("bigint"), CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
        }
    }
}
