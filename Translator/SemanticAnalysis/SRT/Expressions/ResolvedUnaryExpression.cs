using Translator.ObjectModel;

namespace Translator.SRT
{
    internal sealed class ResolvedUnaryExpression : ResolvedExpression
    {
        public ResolvedUnaryExpression(UnaryOperation operation, ResolvedExpression operand)
        {
            Operation = operation;
            Operand = operand;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.UnaryExpression;
        public override ObjectTypes Type => Operand.Type;
        public UnaryOperation Operation { get; }
        public ResolvedExpression Operand { get; }
    }
}
