namespace AParser
{
    public class SqlServerToCharTranslator : ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // TO_CHAR(123) => CONVERT(nvarchar, 123)
            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "convert", CreateFunctionParameterNode("nvarchar"), CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
        }
    }
}
