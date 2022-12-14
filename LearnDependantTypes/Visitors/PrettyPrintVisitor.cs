using System.Collections.Generic;

namespace LearnDependantTypes.Visitors
{
    public class PrettyPrintVisitor : IExprVisitor<string>, ITopLevelVisitor<string>, IStatementVisitor<string>
    {
        private string Indent(string with, string input)
        {
            return $"{with}{input.Replace("\n", "\n" + with)}";
        }

        private static Dictionary<BinaryOperation.Bop, string> _lt = new Dictionary<BinaryOperation.Bop, string>()
        {
            {BinaryOperation.Bop.Equals, "=="},
            {BinaryOperation.Bop.NotEquals, "!="},
            {BinaryOperation.Bop.Plus, "+="},
            {BinaryOperation.Bop.Minus, "-="},
        };
        public string VisitBinaryOperation(BinaryOperation bop)
        {
            return $"({this.VisitExpr(bop.Lhs)} {_lt[bop.Op]} {this.VisitExpr(bop.Rhs)})";
        }

        public string VisitBoolean(Boolean boolean)
        {
            return boolean.Value + "";
        }

        public string VisitFuncCall(FuncCall fnCall)
        {
            return $"{this.VisitExpr(fnCall.Callee)}({string.Join(", ", fnCall.Arguments)})";
        }

        public string VisitIfElse(IfElse ie)
        {
            string tmp = ie.FalseBLock.HasValue ? this.VisitStatement(ie.FalseBLock.Value) : "";
            return $"if {this.VisitExpr(ie.Conditional)} {this.VisitStatement(ie.TrueBlock)} {ie.ElseToken?.Lexeme} {tmp}";
        }

        public string VisitInteger(Integer i)
        {
            return i.Value + "";
        }

        public string VisitUnaryOperation(UnaryOperation uop)
        {
            return uop.Op == UnaryOperation.Uop.Negate? "!":"-" + $"({this.VisitExpr(uop.Expr)})";
        }

        public string VisitVarGet(VarGet vg)
        {
            return vg.Name;
        }

        public string VisitVarSet(VarSet vs)
        {
            return $"{vs.Name} = {this.VisitExpr(vs.Expr)}";
        }

        public string VisitStruct(Struct s)
        {
            throw new System.NotImplementedException();
        }

        public string FnDeclTopLevel(FnDeclTopLevel s)
        {
            return $"{this.VisitStatement(s.Function)}";
        }

        public string VisitBlock(Block block)
        {
            List<string> tmp = new List<string>();
            foreach (var statement in block.Statements)
            {
                tmp.Add(this.VisitStatement(statement));
            }
            return "{\n" + Indent("\t", string.Join("\n", tmp)) + "\n}";
        }

        public string VisitExprStatement(ExprStatement exprStatement)
        {
            return this.VisitExpr(exprStatement.Expr);
        }

        public string VisitFnDecl(FnDecl funcDecl)
        {
            string args = "";
            foreach (var tuple in funcDecl.ParametersAndType)
            {
                args += $"{tuple.Item1}:{tuple.Item2}, ";
            }
            
            return $"fn {funcDecl.Name}({args}) -> {funcDecl.RetType} {this.VisitStatement(funcDecl.FunctionBody)}";
        }

        public string VisitReturn(Return ret)
        {
            return $"return {this.VisitExpr(ret.Expr)}";
        }

        public string VisitVarDecl(VarDecl decl)
        {
            return $"var {decl.Identifier}" + decl.TypeAnnotation + $" = {this.VisitExpr(decl.Expr)}";
        }
    }
}