namespace LearnDependantTypes
{
    public class InterpreterVisitor : IExprVisitor<IInterpreterValue>, ITopLevelVisitor<IInterpreterValue>, IStatementVisitor<IInterpreterValue>
    {
        public IInterpreterValue VisitBinaryOperation(BinaryOperation bop)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitBoolean(Boolean boolean)
        {
            return new BooleanValue(boolean.Value);
        }

        public IInterpreterValue VisitFuncCall(FuncCall fnCall)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitIdentifier(Identifier id)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitIfElse(IfElse ie)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitInteger(Integer i)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitUnaryOperation(UnaryOperation uop)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitVarGet(VarGet vg)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitVarSet(VarSet vs)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitStruct(Struct s)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue FnDeclTopLevel(FnDeclTopLevel s)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitBlock(Block block)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitExprStatement(ExprStatement exprStatement)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitFnDecl(FnDecl funcDecl)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitReturn(Return ret)
        {
            throw new System.NotImplementedException();
        }

        public IInterpreterValue VisitVarDecl(VarDecl decl)
        {
            throw new System.NotImplementedException();
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
}