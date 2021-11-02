using System;
using System.Linq;
using Translator;
using Translator.AST;

namespace language
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();
            var tokens = lexer.Tokenize("12 * 2 / 6");

            var parser = new Parser();
            var expression = parser.Parse(tokens);

            var evaluator = new Evaluator();
            Console.WriteLine(evaluator.Evaluate(expression));

            foreach (var error in lexer.Errors.Concat(parser.Errors))
                Console.WriteLine(error);


            Console.ReadLine();
        }
    }
}
