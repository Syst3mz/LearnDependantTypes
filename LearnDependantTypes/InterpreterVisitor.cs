using System;
using System.Collections.Generic;

namespace LearnDependantTypes
{
    public class InterpreterVisitor : IExprVisitor<IInterpreterValue>, ITopLevelVisitor<IInterpreterValue>, IStatementVisitor<IInterpreterValue>
    {
        private InterpreterEnvironment _ie = new InterpreterEnvironment();
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
            InterpreterEnvironment prior = _ie;
            var def = prior.Get(fnCall.Callee)
            InterpreterEnvironment func = new InterpreterEnvironment(_ie);
            foreach (var expr in fnCall.Arguments)
            {
                func.Bind();
            }
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
            InterpreterEnvironment prior = _ie;
            _ie = new InterpreterEnvironment(_ie);
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
            _ie.Bind(funcDecl.Name.Value, new FunctionValue(funcDecl.Name.Value, funcDecl.FunctionBody, argNames));
            return new UnitValue();
        }

        public IInterpreterValue VisitReturn(Return ret)
        {
            return this.VisitExpr(ret.Expr);
        }

        public IInterpreterValue VisitVarDecl(VarDecl decl)
        {
            _ie.Bind(decl.Identifier.Value, this.VisitExpr(decl.Expr));
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
    }
    
    public class IntegerValue : IInterpreterValue 
    {
        public long Value;

        public IntegerValue(long value)
        {
            Value = value;
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
    }

    public struct UnitValue : IInterpreterValue
    {
    }
}