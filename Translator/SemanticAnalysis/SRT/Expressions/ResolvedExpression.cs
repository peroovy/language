using Translator.ObjectModel;

namespace Translator.SRT
{
    internal abstract class ResolvedExpression : ResolvedNode
    {
        public abstract ObjectTypes Type { get; }
    }
}
