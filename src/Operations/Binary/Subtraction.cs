using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Subtraction : NumberBinaryOperation
    {
        private Subtraction() { }

        static Subtraction()
        {
            Instance = new Subtraction();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Subtraction;

        public static Subtraction Instance { get; }

        public override Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Int)
                return (Int)left - (Int)right;

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Float)
                return (Int)left - (Float)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Int)
                return (Float)left - (Int)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Float)
                return (Float)left - (Float)right;

            throw new System.InvalidOperationException();
        }
    }
}
