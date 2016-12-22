namespace AParser
{
    public class OracleModTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // MOD(3, 5) translated to: mod(floor(3), floor(5))
            Translator = translator;
            NodeFactory = nodeFactory;

            IASTNode newParam1 = NodeFactory.CreateFunctionParameterNode();
            AddFunction(newParam1.SubNodes, "floor", CreateFunctionParameterNodeWithSubNodes(subNodes[1]));
            IASTNode newParam2 = NodeFactory.CreateFunctionParameterNode();
            AddFunction(newParam2.SubNodes, "floor", CreateFunctionParameterNodeWithSubNodes(subNodes[3]));

            AddFunction(newNodes, "mod", newParam1, newParam2);
        }
    }
}
