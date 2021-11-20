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

                var compiler = new Compiler(code);
                var value = compiler.Compile();
                var errors = compiler.Errors;

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
