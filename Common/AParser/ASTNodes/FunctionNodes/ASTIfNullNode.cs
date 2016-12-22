namespace AParser
{
    public class ASTIfNullNode : ASTFunctionNode
    {
        public ASTIfNullNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "ifnull"; } }
    }
}
