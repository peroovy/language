using System.Collections.Generic;
using System.Linq;
using Translator;
using Translator.ObjectModel;

namespace language
{
    internal sealed class Compiler
    {
        private readonly Lexer _lexer = new Lexer();
        private readonly Parser _parser = new Parser();
        private readonly SemanticResolver _resolver = new SemanticResolver();
        private readonly Evaluator _evaluator = new Evaluator();

        private readonly Dictionary<Variable, Object> _scope;

        public Compiler(Dictionary<Variable, Object> scope)
        {
            _scope = scope;
        }

        public CompilationState<Object> Compile(SourceCode code)
        {
            var lexicalAnalysis = _lexer.Tokenize(code);
            var syntaxAnalysis = _parser.Parse(code, lexicalAnalysis.Representation);
            var semanticAnalysis = _resolver.Resolve(code, syntaxAnalysis.Representation, _scope);
            var evaluation = _evaluator.Evaluate(code, semanticAnalysis.Representation, _scope);

            var errors = lexicalAnalysis.Errors
                .Concat(syntaxAnalysis.Errors)
                .Concat(semanticAnalysis.Errors)
                .Concat(evaluation.Errors);

            return new CompilationState<Object>(evaluation.Representation, errors);
        }
    }
}
