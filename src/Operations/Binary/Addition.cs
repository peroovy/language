using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Addition : NumberBinaryOperation
    {
        private Addition() { }

        static Addition()
        {
            Instance = new Addition();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Addition;

        public static Addition Instance { get; }

        public override Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Int)
                return (Int)left + (Int)right;

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Float)
                return (Int)left + (Float)right;    

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Int)
                return (Float)left + (Int)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Float)
                return (Float)left + (Float)right;
            
            throw new System.InvalidOperationException();
        }
    }
}
