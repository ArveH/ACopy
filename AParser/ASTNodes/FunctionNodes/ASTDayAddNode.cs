namespace AParser
{
    public class ASTDayAddNode: ASTFunctionNode
    {
        public ASTDayAddNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "dayadd"; } }
    }
}
