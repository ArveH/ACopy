namespace AParser
{
    public class ASTMaxDateNode : ASTNodeBase
    {
        public ASTMaxDateNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "max_date"; } }
    }
}
