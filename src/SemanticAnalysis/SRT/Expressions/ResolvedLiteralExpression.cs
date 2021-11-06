using System;

namespace Translator.SRT
{
    internal sealed class ResolvedLiteralExpression : ResolvedExpression
    {
        public ResolvedLiteralExpression(string value, Type type)
        {
            Value = value;
            ReturnedType = type;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.LiteralExpression;
        public override Type ReturnedType { get; }
        public string Value { get; }
    }
}
