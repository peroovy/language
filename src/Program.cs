using System;
using System.Collections.Generic;
using System.Linq;
using Translator;
using Translator.ObjectModel;

namespace language
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var scope = new Dictionary<Variable, Translator.ObjectModel.Object>();

            while (true)
            {
                Console.Write(">> ");
                var code = new SourceCode(Console.ReadLine());

                var compiler = new Compiler(scope);
                var compilation = compiler.Compile(code);

                var value = compilation.Representation;
                var errors = compilation.Errors;

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
