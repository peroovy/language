using System;

namespace Translator.SRT
{
    internal sealed class ResolvedBinaryExpression : ResolvedExpression
    {
        public ResolvedBinaryExpression(ResolvedExpression left, 
            BinaryOperation operation, ResolvedExpression right)
        {
            Left = left;
            Operation = operation;
            Right = right;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.BinaryExpression;
        public override Type ReturnedType => Operation.ReturnedType;
        public ResolvedExpression Left { get; }
        public BinaryOperation Operation { get; }
        public ResolvedExpression Right { get; }
    }
}
