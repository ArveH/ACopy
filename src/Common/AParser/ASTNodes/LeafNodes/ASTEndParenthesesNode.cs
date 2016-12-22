namespace AParser
{
    public class ASTEndParenthesesNode: ASTNodeBase
    {
        public ASTEndParenthesesNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return ")"; } }
    }
}
