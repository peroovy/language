using Translator.ObjectModel;

namespace Translator.SRT
{
    internal sealed class ResolvedUnaryExpression : ResolvedExpression
    {
        public ResolvedUnaryExpression(IUnaryOperation operation, ResolvedExpression operand)
        {
            Operation = operation;
            Operand = operand;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.UnaryExpression;
        public override ObjectTypes Type => Operand.Type;
        public IUnaryOperation Operation { get; }
        public ResolvedExpression Operand { get; }
    }
}
