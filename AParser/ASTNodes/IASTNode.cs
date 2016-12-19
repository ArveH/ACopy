namespace AParser
{
    public interface IASTNode
    {
        bool AddSpace { get; set; }
        string Text { get; set; }
        string TextUsingAddSpace { get; }
        bool TextEqualTo(string text, bool ignoreCase = true);
        ASTNodeList SubNodes { get; }
        IASTNode Parse(IASTNodeFactory nodeFactory, IAParser aParser, ATokens tokens);
        IASTNode CloneWithoutSubNodes();
        string ToString();
        void ResetSubNodes();
    }
}
