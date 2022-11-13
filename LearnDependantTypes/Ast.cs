using System.Collections.Generic;

namespace LearnDependantTypes
{
    public interface IAstNode
    {
        public Location Location { get; set; }
    }

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
        //todo deal with later
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

        public Identifier Name;
        public List<(Identifier, Identifier)> ParametersAndType;
        public Block FunctionBody;
        public Identifier RetType;

        public FnDecl(Identifier name, List<(Identifier, Identifier)> parametersAndType, Block functionBody, Identifier retType)
        {
            Name = name;
            ParametersAndType = parametersAndType;
            FunctionBody = functionBody;
            RetType = retType;
        }
    }

    public struct Return : IAstStatement
    {
        public IAstExpr Expr;

        public Return(IAstExpr expr)
        {
            Expr = expr;
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

    public struct VarDecl : IAstStatement
    {
        public Identifier Identifier;
        public Identifier? TypeAnnotation;
        public IAstExpr Expr;

        public VarDecl(Identifier identifier, Identifier? typeAnnotation, IAstExpr expr)
        {
            Identifier = identifier;
            TypeAnnotation = typeAnnotation;
            Expr = expr;
        }
    }

    public struct IfElse : IAstExpr
    {
        public Token IfToken;
        public IAstExpr Conditional;
        public Block TrueBlock;
        public Token? ElseToken;
        public Block? FalseBLock;

        public IfElse(Token ifToken, IAstExpr conditional, Block trueBlock) : this()
        {
            IfToken = ifToken;
            Conditional = conditional;
            TrueBlock = trueBlock;
            ElseToken = null;
            FalseBLock = null;
        }

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

    public struct VarGet : IAstExpr
    {
        public Identifier Name;

        public VarGet(Identifier name)
        {
            Name = name;
        }
    }
    
    public struct VarSet : IAstExpr
    {
        public Identifier Name;
        public IAstExpr Expr;

        public VarSet(Identifier name, IAstExpr expr)
        {
            Name = name;
            Expr = expr;
        }
    }

    public struct FuncCall : IAstExpr
    {
        public IAstExpr Callee;

        public List<IAstExpr> Arguments;

        public FuncCall(IAstExpr callee, List<IAstExpr> arguments)
        {
            Callee = callee;
            Arguments = arguments;
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

        public Identifier(Token token)
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