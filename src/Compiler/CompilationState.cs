using System.Collections.Generic;

namespace Translator
{
    internal sealed class CompilationState<TCodeRepresentation>
    {
        public CompilationState(TCodeRepresentation representation, IEnumerable<Error> errors)
        {
            Representation = representation;
            Errors = errors;
        }

        public TCodeRepresentation Representation { get; }
        public IEnumerable<Error> Errors { get; }
    }
}
