using System.Collections.Generic;

namespace LearnDependantTypes
{
    public class Parser
    {
        private ParserStream _ps;
        public Parser(List<Token> toParse)
        {
            _ps = new ParserStream(toParse);
        }

        public Parser(ParserStream ps)
        {
            _ps = ps;
        }

        public List<IAstTopLevel> Parse()
        {
            
        }
    }
}