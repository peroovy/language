using Translator.ObjectModel;

namespace Translator.SRT
{
    internal sealed class ResolvedLiteralExpression : ResolvedExpression
    {
        public ResolvedLiteralExpression(string value, ObjectTypes type)
        {
            Value = value;
            Type = type;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.LiteralExpression;
        public override ObjectTypes Type { get; }
        public string Value { get; }
    }
}
