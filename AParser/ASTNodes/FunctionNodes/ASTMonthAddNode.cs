namespace AParser
{
    public class ASTMonthAddNode: ASTFunctionNode
    {
        public ASTMonthAddNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "monthadd"; } }
    }
}
