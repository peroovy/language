using System;
using System.Linq;
using Translator;

namespace language
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">> ");
                var code = new SourceCode(Console.ReadLine());

                var lexer = new Lexer(code);
                var parser = new Parser(code, lexer.Tokenize());
                var resolver = new SemanticResolver(code, parser.Parse());
                var evaluator = new Evaluator(code, resolver.Resolve());

                var value = evaluator.Evaluate();

                var errors = lexer.Errors
                    .Concat(parser.Errors)
                    .Concat(resolver.Errors)
                    .Concat(evaluator.Errors);

                if (!errors.Any())
                    Console.WriteLine(value);

                
                foreach (var error in errors)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(error.Message);

                    Console.Write(new string(' ', 6));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(error.Code.Substring(0, error.Span.Start));

                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    var a = error.Code.Length;
                    Console.Write(error.Code.Substring(error.Span.Start, error.Span.Length));
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(error.Code.Substring(error.Span.End));

                    Console.ResetColor();
                    Console.WriteLine();
                }              
            }
        }
    }
}
