namespace AParser
{
    public class SqlServerToIntTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // TO_int('123') => CONVERT(int, '123')
            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "convert", CreateFunctionParameterNode("int"), CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
        }
    }
}
