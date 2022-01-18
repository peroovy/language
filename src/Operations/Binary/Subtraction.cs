using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Subtraction : NumberBinaryOperation
    {
        public override BinaryOperationKind Kind => BinaryOperationKind.Subtraction;

        public override Object Evaluate(Object left, Object right)
        {
            if (left.Kind == ObjectTypes.Int && right.Kind == ObjectTypes.Int)
                return (Int)left - (Int)right;

            if (left.Kind == ObjectTypes.Int && right.Kind == ObjectTypes.Float)
                return (Int)left - (Float)right;

            if (left.Kind == ObjectTypes.Float && right.Kind == ObjectTypes.Int)
                return (Float)left - (Int)right;

            if (left.Kind == ObjectTypes.Float && right.Kind == ObjectTypes.Float)
                return (Float)left - (Float)right;

            throw new System.InvalidOperationException();
        }
    }
}
