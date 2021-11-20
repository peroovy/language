using System.Linq;
using Translator;

namespace language
{
    internal sealed class Compiler
    {
        private readonly Lexer _lexer = new Lexer();
        private readonly Parser _parser = new Parser();
        private readonly SemanticResolver _resolver = new SemanticResolver();
        private readonly Evaluator _evaluator = new Evaluator();

        public CompilationState<object> Compile(SourceCode code)
        {
            var lexicalAnalysis = _lexer.Tokenize(code);
            var syntaxAnalysis = _parser.Parse(code, lexicalAnalysis.Representation);
            var semanticAnalysis = _resolver.Resolve(code, syntaxAnalysis.Representation);

            var value = _evaluator.Evaluate(semanticAnalysis.Representation);

            var errors = lexicalAnalysis.Errors
                .Concat(syntaxAnalysis.Errors)
                .Concat(semanticAnalysis.Errors);

            return new CompilationState<object>(value, errors);
        }
    }
}
