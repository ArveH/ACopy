namespace AParser
{
    public class ASTToFloatNode: ASTFunctionNode
    {
        public ASTToFloatNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "to_float"; } }
    }
}
