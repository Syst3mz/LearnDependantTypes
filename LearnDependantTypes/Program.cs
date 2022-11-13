using System;

namespace LearnDependantTypes
{
    class Program
    {
        public static string DemoProgFull = "struct Void {}" +
                                        "struct V {" +
                                        "   var A:bool;" +
                                        "   var B:int;" +
                                        "   " +
                                        "   fn RetA() -> bool {" +
                                        "       return A;" +
                                        "   }" +
                                        "   " +
                                        "   fn AddToB(val:int) -> int {" +
                                        "       B = B + val;" +
                                        "       return B;" +
                                        "   }" +
                                        "}" +
                                        "fn main() -> Void {" +
                                        "   var x = 0;" +
                                        "   x = x + 1;" +
                                        "   if x == 20 {" +
                                        "       print(123);" +
                                        "   } else {" +
                                        "       print(456);" +
                                        "   }" +
                                        "}";

        public static string DemoProg = "fn func() -> int {\n" +
                                        "   return 0;\n" +
                                        "}\n" +
                                        "fn main() -> bool {\n" +
                                        "   var x = 20;\n" +
                                        "   x = 14;\n" +
                                        "   var y = func();\n" +
                                        "   return if x == 4 {true} else {false};\n" +
                                        "}";
        
        static void Main(string[] args)
        {
            var lexed = new Lexer(DemoProg).Lex();
            var parsed = new Parser(lexed).Parse();
            var prettyPrinter = new PrettyPrintVisitor();
            foreach (var level in parsed)
            {
                Console.WriteLine(prettyPrinter.VisitTopLevel(level));
            }

            InterpreterVisitor interpreter = new InterpreterVisitor();
            Console.WriteLine(interpreter.Interpret(parsed));
            
            return;
        }
    }
}