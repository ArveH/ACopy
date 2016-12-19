namespace AParser
{
    public class SqlServerGuid2StrTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // GUID2STR(guid_col) => lower(guid_col)

            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "lower", CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
        }
    }
}
