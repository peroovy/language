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
            var evaluator = new Evaluator();

            while (true)
            {
                Console.Write(">> ");
                var code = Console.ReadLine();

                var tokens = lexer.Tokenize(code);
                var expression = parser.Parse(tokens);
                var value = evaluator.Evaluate(expression);

                var errors = lexer.Errors.Concat(parser.Errors);

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
