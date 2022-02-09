using System.Linq;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Greater : ComparableBinaryOperation
    {
        private Greater() { }

        static Greater()
        {
            Instance = new Greater();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Greater;

        public static Greater Instance { get; }

        public override Object Evaluate(Int left, Int right) => new Bool(left.Value > right.Value);
        
        public override Object Evaluate(Int left, Float right) => new Bool(left.Value > right.Value);

        public override Object Evaluate(Int left, Long right)
        {
            var longLeft = (Long)ImplicitCasting.Instance.Apply(left, ObjectTypes.Long);

            return Evaluate(longLeft, right);
        }


        public override Object Evaluate(Float left, Int right) => new Bool(left.Value > right.Value);

        public override Object Evaluate(Float left, Float right) => new Bool(left.Value > right.Value);

        public override Object Evaluate(Float left, Long right)
        {
            throw new System.NotImplementedException();
        }


        public override Object Evaluate(Long left, Int right)
        {
            var longRight = (Long)ImplicitCasting.Instance.Apply(right, ObjectTypes.Long);

            return Evaluate(left, longRight);
        }

        public override Object Evaluate(Long left, Float right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Long right)
        {
            var value = Evaluate(left.Chunks.ToArray(), left.IsNegative, right.Chunks.ToArray(), right.IsNegative);

            return new Bool(value);
        }

        public bool Evaluate(long[] left, bool isNegativeLeft, long[] right, bool isNegativeRight)
        {
            if (isNegativeLeft != isNegativeRight)
                return isNegativeLeft == false;

            if (left.Length != right.Length)
                return left.Length > right.Length;

            for (var i = left.Length - 1; i >= 0; i--)
            {
                if (left[i] != right[i])
                    return left[i] > right[i] ^ isNegativeLeft;
            }

            return true;
        }
    }
}
