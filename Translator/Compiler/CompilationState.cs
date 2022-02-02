using System.Collections.Generic;

namespace Translator
{
    internal sealed class CompilationState<TValue>
    {
        public CompilationState(TValue value, IEnumerable<Error> errors)
        {
            Value = value;
            Errors = errors;
        }

        public TValue Value { get; }
        public IEnumerable<Error> Errors { get; }
    }
}
