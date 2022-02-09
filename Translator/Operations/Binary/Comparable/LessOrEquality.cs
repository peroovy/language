using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LessOrEquality : ComparableBinaryOperation
    {
        private LessOrEquality() { }

        static LessOrEquality()
        {
            Instance = new LessOrEquality();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.LessOrEquality;

        public static LessOrEquality Instance { get; }

        public override Object Evaluate(Object left, Object right) =>
            LogicalNegation.Instance.Evaluate((Bool)Greater.Instance.Evaluate(left, right));
    }
}
