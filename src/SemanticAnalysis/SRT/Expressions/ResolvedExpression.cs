using System;

namespace Translator.SRT
{
    internal abstract class ResolvedExpression : ResolvedNode
    {
        public abstract Type ReturnedType { get; }
    }
}
