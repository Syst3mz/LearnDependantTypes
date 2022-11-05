using System.Collections.Generic;

namespace LearnDependantTypes
{
    public interface IAstTopLevel
    {
        
    }
    
    public interface IAstStatement
    {
    }

    public interface IAstExpr
    {
    }

    public struct Struct : IAstTopLevel
    {
        // deal with later
    }

    public struct FnDeclTopLevel : IAstTopLevel
    {
        public FnDecl Function;

        public FnDeclTopLevel(FnDecl function)
        {
            Function = function;
        }
    }
    
    public struct Block : IAstStatement
    {
        public List<IAstStatement> Statements;

        public Block(List<IAstStatement> statements)
        {
            Statements = statements;
        }
    }
    
    public struct FnDecl : IAstStatement
    {
        public Token NameToken;
        public string Name => NameToken.Lexeme;

        public List<(Token, Token)> Arguments;
        public Block FunctionBody;

        public FnDecl(Token nameToken, List<(Token, Token)> arguments, Block functionBody)
        {
            NameToken = nameToken;
            Arguments = arguments;
            FunctionBody = functionBody;
        }
    }

    public struct ExprStatement : IAstStatement
    {
        public IAstExpr Expr;

        public ExprStatement(IAstExpr expr)
        {
            Expr = expr;
        }
    }

    public struct VarDecl : IAstExpr
    {
        public Token NameToken;
        public string Name => NameToken.Lexeme;
        public IAstExpr Expr;

        public VarDecl(Token nameToken, IAstExpr expr)
        {
            NameToken = nameToken;
            Expr = expr;
        }
    }

    // ifs

    public struct If : IAstExpr
    {
        public Token IfToken;
        public IAstExpr Conditional;
        public Block TrueBlock;

        public If(Token ifToken, IAstExpr conditional, Block trueBlock)
        {
            IfToken = ifToken;
            Conditional = conditional;
            TrueBlock = trueBlock;
        }
    }
    
    public struct IfElse : IAstExpr
    {
        public Token IfToken;
        public IAstExpr Conditional;
        public Block TrueBlock;
        public Token ElseToken;
        public Block FalseBLock;

        public IfElse(Token ifToken, IAstExpr conditional, Block trueBlock, Token elseToken, Block falseBLock)
        {
            IfToken = ifToken;
            Conditional = conditional;
            TrueBlock = trueBlock;
            ElseToken = elseToken;
            FalseBLock = falseBLock;
        }
    }

    public struct UnaryOperation : IAstExpr
    {
        public enum Uop
        {
            Negate,
            MakeNegative
        }

        public Token Token;
        public IAstExpr Expr;
        public Uop Op;

        public UnaryOperation(Token token, IAstExpr expr, Uop op)
        {
            Token = token;
            Expr = expr;
            Op = op;
        }
    }

    public struct BinaryOperation : IAstExpr
    {
        public enum Bop
        {
            Plus,
            Minus,
            Equals,
            NotEquals,
        }

        public Token Token;
        public IAstExpr Lhs, Rhs;
        public Bop Op;

        public BinaryOperation(Token token, IAstExpr lhs, IAstExpr rhs, Bop op)
        {
            Token = token;
            Lhs = lhs;
            Rhs = rhs;
            Op = op;
        }
    }

    // simple AST nodes
    
    public struct Integer : IAstExpr
    {
        public Token Token;
        public long Value;

        public Integer(Token token, long value)
        {
            Token = token;
            Value = value;
        }
    }
    
    public struct Identifier : IAstExpr
    {
        public Token Token;
        public string Value => Token.Lexeme;

        public Identifier(Token token, long value)
        {
            Token = token;
        }
    }

    public struct Boolean : IAstExpr
    {
        public Token Token;
        public bool Value;

        public Boolean(Token token, bool value)
        {
            Token = token;
            Value = value;
        }
    }
}