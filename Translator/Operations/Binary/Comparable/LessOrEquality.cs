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
