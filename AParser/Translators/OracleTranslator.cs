using System.Collections.Generic;

namespace AParser
{
    public class OracleTranslator: ATranslator
    {
        public OracleTranslator(IASTNodeFactory nodeFactory)
        {
            NodeFactory = nodeFactory;
            NodeTranslators = new Dictionary<string, IASTNodeTranslator>
            {
                {ASTDayAddNode.KeyWord,           new OracleDayAddTranslator()},
                {ASTGetDateNode.KeyWord,          new OracleGetDateTranslator()},
                {ASTGuid2StrNode.KeyWord,         new OracleGuid2StrTranslator()},
                {ASTIfNullNode.KeyWord,           new OracleIfNullTranslator()},
                {ASTMaxDateNode.KeyWord,          new OracleMaxDateTranslator()},
                {ASTMinDateNode.KeyWord,          new OracleMinDateTranslator()},
                {ASTModNode.KeyWord,              new OracleModTranslator()},
                {ASTMonthAddNode.KeyWord,         new OracleMonthAddTranslator()},
                {ASTToCharNode.KeyWord,           new OracleToCharTranslator()},
                {ASTToCounterNode.KeyWord,        new OracleToCounterTranslator()},
                {ASTToFloatNode.KeyWord,          new OracleToFloatTranslator()},
                {ASTToIntNode.KeyWord,            new OracleToIntTranslator()}
            };
        }
    }
}
