namespace AParser
{
    public interface IATranslator
    {
        ASTNodeList Translate(ASTNodeList nodes);
        void TranslateSubNodes(ASTNodeList newNodes, IASTNode fromNode);
    }
}
