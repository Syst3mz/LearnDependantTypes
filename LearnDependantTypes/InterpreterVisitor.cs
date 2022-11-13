using System;
using System.Collections.Generic;

namespace LearnDependantTypes
{
    public class InterpreterVisitor : IExprVisitor<IInterpreterValue>, ITopLevelVisitor<IInterpreterValue>, IStatementVisitor<IInterpreterValue>
    {
        private InterpreterEnvironment<IInterpreterValue> _ie = new InterpreterEnvironment<IInterpreterValue>();
        public IInterpreterValue VisitBinaryOperation(BinaryOperation bop)
        {

            var lhs = this.VisitExpr(bop.Lhs);
            var rhs = this.VisitExpr(bop.Rhs);

            if (lhs is IntegerValue i1 && rhs is IntegerValue i2)
            {
                switch (bop.Op)
                {
                    case BinaryOperation.Bop.Plus:
                        return new IntegerValue(i1.Value + i2.Value);
                    case BinaryOperation.Bop.Minus:
                        return new IntegerValue(i1.Value - i2.Value);
                    case BinaryOperation.Bop.Equals:
                        return new BooleanValue(i1.Value == i2.Value);
                    case BinaryOperation.Bop.NotEquals:
                        return new BooleanValue(i1.Value != i2.Value);
                }
            }
            else
            {
                switch (bop.Op)
                {
                    case BinaryOperation.Bop.Equals:
                        return new BooleanValue(lhs == rhs);
                    case BinaryOperation.Bop.NotEquals:
                        return new BooleanValue(lhs != rhs);
                }
            }

            throw new Exception("Invalid types to add or subtract");
        }

        public IInterpreterValue VisitBoolean(Boolean boolean)
        {
            return new BooleanValue(boolean.Value);
        }

        public IInterpreterValue VisitFuncCall(FuncCall fnCall)
        {
            InterpreterEnvironment<IInterpreterValue> prior = _ie;
            InterpreterEnvironment<IInterpreterValue> func = new InterpreterEnvironment<IInterpreterValue>(_ie);

            var evalCall = this.VisitExpr(fnCall.Callee);
            if (evalCall is FunctionValue fv)
            {
                for (int i = 0; i < fv.Arguments.Count; i++)
                {
                    func.Bind(fv.Arguments[i], this.VisitExpr(fnCall.Arguments[i]));
                }

                _ie = func;
                try
                {
                    var ret = this.VisitStatement(fv.FuncBlock);
                }
                catch (ReturnPseudoException re)
                {
                    return re.RetVal;
                }
                _ie = prior;
                return new UnitValue();
            }
            return new UnitValue();
        }

        public IInterpreterValue VisitIdentifier(Identifier id)
        {
            throw new Exception("There should not be a random identifier but there is");
        }

        public IInterpreterValue VisitIfElse(IfElse ie)
        {
            var conditional = (BooleanValue) this.VisitExpr(ie.Conditional);
            if (conditional.Value)
            {
                return this.VisitStatement(ie.TrueBlock);
            }
            else if (ie.FalseBLock.HasValue)
            {
                return this.VisitStatement(ie.FalseBLock.Value);
            }

            return new UnitValue();
        }

        public IInterpreterValue VisitInteger(Integer i)
        {
            return new IntegerValue(i.Value);
        }

        public IInterpreterValue VisitUnaryOperation(UnaryOperation uop)
        {
            switch (uop.Op)
            {
                case UnaryOperation.Uop.Negate:
                    return new BooleanValue(! ((BooleanValue) this.VisitExpr(uop.Expr)).Value);
                case UnaryOperation.Uop.MakeNegative:
                    return new IntegerValue(- ((IntegerValue) this.VisitExpr(uop.Expr)).Value);
                default:
                    throw new Exception("This exception is quite literally imposible but is in visit UOP");
            }
        }

        public IInterpreterValue VisitVarGet(VarGet vg)
        {
            return _ie.Get(vg.Name.Value);
        }

        public IInterpreterValue VisitVarSet(VarSet vs)
        {
            var e = this.VisitExpr(vs.Expr);
            _ie.Set(vs.Name.Value, e);
            return e;
        }

        public IInterpreterValue VisitStruct(Struct s)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue FnDeclTopLevel(FnDeclTopLevel s)
        {
            return this.VisitStatement(s.Function);
        }

        public IInterpreterValue VisitBlock(Block block)
        {
            InterpreterEnvironment<IInterpreterValue> prior = _ie;
            _ie = new InterpreterEnvironment<IInterpreterValue>(_ie);
            IInterpreterValue lastExpr = new UnitValue();
            foreach (var statement in block.Statements)
            {
                lastExpr = this.VisitStatement(statement);
            }


            _ie = prior;
            return lastExpr;
        }

        public IInterpreterValue VisitExprStatement(ExprStatement exprStatement)
        {
            return this.VisitExpr(exprStatement.Expr);
        }

        public IInterpreterValue VisitFnDecl(FnDecl funcDecl)
        {
            List<string> argNames = new List<string>();
            foreach (var tuple in funcDecl.ParametersAndType)
            {
                argNames.Add(tuple.Item1.Value);
            }

            var fn = new FunctionValue(funcDecl.Name.Value, funcDecl.FunctionBody, argNames);
            _ie.Bind(funcDecl.Name.Value, fn);
            return fn;
        }

        public class ReturnPseudoException : Exception
        {
            public IInterpreterValue RetVal;

            public ReturnPseudoException(IInterpreterValue retVal)
            {
                RetVal = retVal;
            }
        }

        public IInterpreterValue VisitReturn(Return ret)
        {
            throw new ReturnPseudoException(this.VisitExpr(ret.Expr));
        }

        public IInterpreterValue VisitVarDecl(VarDecl decl)
        {
            _ie.Bind(decl.Identifier.Value, this.VisitExpr(decl.Expr));
            return new UnitValue();
        }

        public IInterpreterValue Interpret(List<IAstTopLevel> tops)
        {
            foreach (var top in tops)
            {
                this.VisitTopLevel(top);
            }

            foreach (var top in tops)
            {
                if (top is FnDeclTopLevel dc)
                {
                    if (dc.Function.Name.Value.Equals("main"))
                    {
                        var main = (FunctionValue)_ie.Get("main");
                        return this.VisitExpr(new FuncCall(new VarGet(new Identifier(new Token(TokenType.Identifier, "main", 0, 0))), new List<IAstExpr>()));
                    }
                }
            }

            return new UnitValue();
        }
    }

    public interface IInterpreterValue
    {
    }

    public class BooleanValue : IInterpreterValue 
    {
        public bool Value;

        public BooleanValue(bool value)
        {
            Value = value;
        }
        
        public override string ToString()
        {
            return Value + "";
        }
    }
    
    public class IntegerValue : IInterpreterValue 
    {
        public long Value;

        public IntegerValue(long value)
        {
            Value = value;
        }
        
        public override string ToString()
        {
            return Value + "";
        }
    }

    public struct FunctionValue : IInterpreterValue
    {
        public string Name;
        public Block FuncBlock;
        public List<string> Arguments;

        public FunctionValue(string name, Block funcBlock, List<string> arguments)
        {
            Name = name;
            FuncBlock = funcBlock;
            Arguments = arguments;
        }
        
        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public struct UnitValue : IInterpreterValue
    {
        public override string ToString()
        {
            return "Unit";
        }
    }
}