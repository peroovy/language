using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Less : ComparableBinaryOperation
    {
        private Less() { }

        static Less()
        {
            Instance = new Less();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Less;

        public static Less Instance { get; }

        public override Object Evaluate(Object left, Object right) =>
            LogicalNegation.Instance.Evaluate((Bool)GreaterOrEquality.Instance.Evaluate(left, right));
    }
}
