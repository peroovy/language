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
