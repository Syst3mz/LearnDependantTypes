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

        public static string DemoProg = "fn main() -> bool {" +
                                        "   var x = 20" +
                                        "   x = 14" +
                                        "   var y = func()" +
                                        "   return if x == 4 {true} else {false};" +
                                        "}";
        
        static void Main(string[] args)
        {
            var lexed = new Lexer(DemoProg).Lex();
            var parsed = new Parser(lexed).Parse();
            return;
        }
    }
}