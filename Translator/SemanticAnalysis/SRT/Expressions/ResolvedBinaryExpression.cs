using Translator.ObjectModel;

namespace Translator.SRT
{
    internal sealed class ResolvedBinaryExpression : ResolvedExpression
    {
        public ResolvedBinaryExpression(ResolvedExpression left, 
            BinaryOperation operation, ResolvedExpression right, TextLocation operatorLocation)
        {
            Left = left;
            Operation = operation;
            Right = right;
            OperatorLocation = operatorLocation;
            Type = operation.GetObjectType(Left.Type, Right.Type);
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.BinaryExpression;
        public override ObjectTypes Type { get; }
        public ResolvedExpression Left { get; }
        public BinaryOperation Operation { get; }
        public ResolvedExpression Right { get; }
        public TextLocation OperatorLocation { get; }
    }
}
