namespace AParser
{
    public class ASTGetDateNode: ASTFunctionNode
    {
        public ASTGetDateNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "getdate"; } }
    }
}
