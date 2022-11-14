using System;
using LearnDependantTypes.Lexing;

namespace LearnDependantTypes.Parsing
{
    public class ParserError : Exception
    {
        public Token ErrTok;

        public ParserError(string message, Token errTok) : base($"{message} ({errTok.Location})")
        {
            ErrTok = errTok;
        }
    }
}