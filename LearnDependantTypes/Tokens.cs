namespace LearnDependantTypes
{
    public enum TokenType
    {
        // static tokens
        Struct,
        If,
        Else,
        Return,
        Fn,
        Var,
        Plus,
        Minus,
        Bang,
        LCurlyBrace, RCurlyBrace, LParen, RParen,
        Equals,
        Where,
        SemiColon,
        Comma,
        Print,
        Arrow,
        EqualsEquals,
        BangEquals,
        Colon,
        Eof,

        // non static tokens
        Integer,
        Boolean,
        Identifier,
        
    }
}