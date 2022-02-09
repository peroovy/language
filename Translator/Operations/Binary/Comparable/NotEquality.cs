using Translator.ObjectModel;

namespace Translator
{
    internal sealed class NotEquality : ComparableBinaryOperation
    {
        private NotEquality() { }

        static NotEquality()
        {
            Instance = new NotEquality();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.NotEquality;

        public static NotEquality Instance { get; }

        public override Object Evaluate(Object left, Object right) =>
            LogicalNegation.Instance.Evaluate((Bool)Equality.Instance.Evaluate(left, right));
    }
}
