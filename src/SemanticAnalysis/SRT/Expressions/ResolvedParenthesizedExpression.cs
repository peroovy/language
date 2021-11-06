using System;

namespace Translator.SRT
{
    internal sealed class ResolvedParenthesizedExpression : ResolvedExpression
    {
        public ResolvedParenthesizedExpression(ResolvedExpression expression)
        {
            Expression = expression;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.ParenthesizedExpression;
        public override Type ReturnedType => Expression.ReturnedType;
        public ResolvedExpression Expression { get; }
    }
}
