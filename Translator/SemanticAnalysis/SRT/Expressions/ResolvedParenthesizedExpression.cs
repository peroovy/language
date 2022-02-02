using Translator.ObjectModel;

namespace Translator.SRT
{
    internal sealed class ResolvedParenthesizedExpression : ResolvedExpression
    {
        public ResolvedParenthesizedExpression(ResolvedExpression expression)
        {
            Expression = expression;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.ParenthesizedExpression;
        public override ObjectTypes Type => Expression.Type;
        public ResolvedExpression Expression { get; }
    }
}
