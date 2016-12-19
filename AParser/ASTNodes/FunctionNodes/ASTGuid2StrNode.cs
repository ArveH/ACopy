namespace AParser
{
    public class ASTGuid2StrNode: ASTFunctionNode
    {
        public ASTGuid2StrNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "guid2str"; } }
    }
}
