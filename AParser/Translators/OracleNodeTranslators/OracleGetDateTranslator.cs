namespace AParser
{
    public class OracleGetDateTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            newNodes.Add(nodeFactory.CreateNode("sysdate"));
        }
    }
}
