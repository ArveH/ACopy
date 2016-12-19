namespace AParser
{
    public interface IASTNodeTranslator
    {
        void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory);
    }
}
