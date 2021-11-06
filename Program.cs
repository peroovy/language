using System;
using System.Linq;
using Translator;

namespace language
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();
            var parser = new Parser();
            var resolver = new SemanticResolver();
            var evaluator = new Evaluator();

            while (true)
            {
                Console.Write(">> ");
                var code = Console.ReadLine();

                var tokens = lexer.Tokenize(code);
                var expression = parser.Parse(tokens);
                var resolvedExpression = resolver.Resolve(expression);
                var value = evaluator.Evaluate(resolvedExpression);

                var errors = lexer.Errors.Concat(parser.Errors).Concat(resolver.Errors).Concat(evaluator.Errors);

                if (!errors.Any())
                    Console.WriteLine(value);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (var error in errors)
                    Console.WriteLine(error);
                Console.ResetColor();
            }
        }
    }
}
