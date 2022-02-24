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

        public override Object Evaluate(Int left, Int right) => Evaluate((Object)left, right);

        public override Object Evaluate(Int left, Float right) => Evaluate((Object)left, right);

        public override Object Evaluate(Int left, Long right) => Evaluate((Object)left, right);

        public override Object Evaluate(Float left, Int right) => Evaluate((Object)left, right);

        public override Object Evaluate(Float left, Float right) => Evaluate((Object)left, right);

        public override Object Evaluate(Float left, Long right) => Evaluate((Object)left, right);

        public override Object Evaluate(Long left, Int right) => Evaluate((Object)left, right);

        public override Object Evaluate(Long left, Float right) => Evaluate((Object)left, right);

        public override Object Evaluate(Long left, Long right) => Evaluate((Object)left, right);
    }
}
