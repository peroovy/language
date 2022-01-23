using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Division : NumberBinaryOperation
    {
        private Division() { }

        static Division()
        {
            Instance = new Division();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Division;

        public static Division Instance { get; }

        public override Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Int)
                return (Int)left / (Int)right;

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Float)
                return (Int)left / (Float)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Int)
                return (Float)left / (Int)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Float)
                return (Float)left / (Float)right;

            throw new System.InvalidOperationException();
        }
    }
}
