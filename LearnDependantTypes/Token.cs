namespace LearnDependantTypes
{
    public struct Token
    {
        public TokenType Type;
        public string Lexeme;
        public Location Location;

        public Token(TokenType type, string lexeme, Location location)
        {
            Type = type;
            Lexeme = lexeme;
            Location = location;
        }
        
        public Token(TokenType type, string lexeme, int linePos, int charPos)
        {
            Type = type;
            Lexeme = lexeme;
            Location = new Location(linePos, charPos);
        }

        public override string ToString()
        {
            return $"[{Type}]:{Lexeme}";
        }
    }

    public struct Location
    {
        public int LinePos, CharPos;

        public Location(int linePos, int charPos)
        {
            LinePos = linePos;
            CharPos = charPos;
        }
    }
}