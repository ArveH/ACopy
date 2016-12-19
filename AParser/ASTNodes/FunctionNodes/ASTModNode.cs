namespace AParser
{
    public class ASTModNode: ASTFunctionNode
    {
        public ASTModNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "mod"; } }
    }
}
