namespace AParser
{
    public interface IAParser
    {
        IASTNodeFactory NodeFactory { get; }
        bool Acceptable(ATokens tokens, string text);
        IASTNode Accept(ATokens tokens, string text);
        ASTNodeList ParseExpression(ATokens tokens);
        ASTNodeList CreateNodeList(string text);
        bool ExpandEmptyStrings { get; set; } // If true, put one space into empty strings. You may want to do this since empty string in Oracle is NULL, 
                                              // and often people compares to NULL instead of empty string without knowing it. Default is false.
    }
}
