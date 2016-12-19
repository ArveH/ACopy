namespace AParser
{
    public class ASTCommaNode: ASTNodeBase
    {
        public ASTCommaNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return ","; } }
    }
}
