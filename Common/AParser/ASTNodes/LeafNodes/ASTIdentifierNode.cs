namespace AParser
{
    public class ASTIdentifierNode: ASTNodeBase
    {
        public ASTIdentifierNode(string text, bool addSpace = true)
        {
            Text = text;
            AddSpace = addSpace;
        }
    }
}
