using Translator.ObjectModel;

namespace Translator
{
    internal sealed class GreaterOrEquality : ComparableBinaryOperation
    {
        private GreaterOrEquality() { }

        static GreaterOrEquality()
        {
            Instance = new GreaterOrEquality();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.GreaterOrEquality;

        public static GreaterOrEquality Instance { get; }

        public override Object Evaluate(Object left, Object right) =>
            LogicalOr.Instance.Evaluate((Bool)Greater.Instance.Evaluate(left, right), 
                                        (Bool)Equality.Instance.Evaluate(left, right));

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
