using System.Collections.Generic;

namespace AParser
{
    public class SqlServerTranslator: ATranslator
    {
        public SqlServerTranslator(IASTNodeFactory nodeFactory)
        {
            NodeFactory = nodeFactory;
            NodeTranslators = new Dictionary<string, IASTNodeTranslator>
            {
                {ASTDayAddNode.KeyWord,           new SqlServerDayAddTranslator()},
                {ASTGetDateNode.KeyWord,          new SqlServerGetDateTranslator()},
                {ASTGuid2StrNode.KeyWord,         new SqlServerGuid2StrTranslator()},
                {ASTIfNullNode.KeyWord,           new SqlServerIfNullTranslator()},
                {ASTMaxDateNode.KeyWord,          new SqlServerMaxDateTranslator()},
                {ASTMinDateNode.KeyWord,          new SqlServerMinDateTranslator()},
                {ASTModNode.KeyWord,              new SqlServerModTranslator()},
                {ASTMonthAddNode.KeyWord,         new SqlServerMonthAddTranslator()},
                {ASTToCharNode.KeyWord,           new SqlServerToCharTranslator()},
                {ASTToCounterNode.KeyWord,        new SqlServerToCounterTranslator()},
                {ASTToFloatNode.KeyWord,          new SqlServerToFloatTranslator()},
                {ASTToIntNode.KeyWord,            new SqlServerToIntTranslator()}
            };
        }
    }
}
