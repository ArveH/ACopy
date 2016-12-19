namespace AParser
{
    public class ASTToIntNode: ASTFunctionNode
    {
        public ASTToIntNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "to_int"; } }
    }
}
