namespace AParser
{
    internal class ASTToCharNode : ASTFunctionNode
    {
        public ASTToCharNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "to_char"; } }
    }
}
