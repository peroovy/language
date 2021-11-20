using System.Collections.Generic;
using System.Linq;
using Translator;

namespace language
{
    internal sealed class Compiler
    {
        public Compiler(SourceCode code)
        {
            Code = code;
        }

        public SourceCode Code { get; }
        public IEnumerable<Error> Errors { get; private set; }

        public object Compile()
        {
            var lexer = new Lexer(Code);
            var tokens = lexer.Tokenize();

            var parser = new Parser(Code, tokens);
            var syntaxTree = parser.Parse();

            var resolver = new SemanticResolver(Code, syntaxTree);
            var semanticTree = resolver.Resolve();

            var evaluator = new Evaluator(Code, semanticTree);
            var value = evaluator.Evaluate();

            Errors = lexer.Errors
                .Concat(parser.Errors)
                .Concat(resolver.Errors)
                .Concat(evaluator.Errors);

            return value;
        }
    }
}
