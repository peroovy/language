using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Equality : ComparableBinaryOperation
    {
        private Equality() { }

        static Equality()
        {
            Instance = new Equality();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Equality;

        public static Equality Instance { get; }

        public override Object Evaluate(Int left, Int right) => new Bool(left.Value == right.Value);

        public override Object Evaluate(Int left, Float right) => new Bool(left.Value == right.Value);

        public override Object Evaluate(Int left, Long right) => Evaluate(right, left);

        public override Object Evaluate(Float left, Int right) => Evaluate(right, left);

        public override Object Evaluate(Float left, Float right) => new Bool(left.Value == right.Value);

        public override Object Evaluate(Float left, Long right) => Evaluate(right, left);

        public override Object Evaluate(Long left, Int right)
        {
            var longRight = ImplicitCasting.Instance.Apply(right, ObjectTypes.Long);

            return Evaluate(left, longRight);
        }

        public override Object Evaluate(Long left, Float right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Long right)
        {
            if (left.IsNegative != right.IsNegative || left.Dimension != right.Dimension)
                return new Bool(false);

            for (var i = 0; i < left.Dimension; i++)
            {
                if (left.Chunks[i] != right.Chunks[i])
                    return new Bool(false);
            }

            return new Bool(true);
        }
    }
}
