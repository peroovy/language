using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedExplicitCastExpression : ResolvedExpression
    {
        public ResolvedExplicitCastExpression(ResolvedExpression expression, ObjectTypes target)
        {
            Expression = expression;
            Target = target;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.ExplicitCastExpression;
        public override ObjectTypes Type => Target;
        public ResolvedExpression Expression { get; }
        public ObjectTypes Target { get; }
    }
}