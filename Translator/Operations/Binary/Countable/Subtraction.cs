using System.Collections.Immutable;
using System.Linq;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Subtraction : NumberBinaryOperation
    {
        private Subtraction() { }

        static Subtraction()
        {
            Instance = new Subtraction();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Subtraction;

        public static Subtraction Instance { get; }

        public override Object Evaluate(Int left, Int right) => new Int(left.Value - right.Value);

        public override Object Evaluate(Int left, Float right) => new Float(left.Value - right.Value);

        public override Object Evaluate(Int left, Long right)
        {
            var longLeft = (Long)ImplicitCasting.Instance.Apply(left, ObjectTypes.Long);

            return Evaluate(longLeft, right);
        }

        public override Object Evaluate(Float left, Int right) => new Int(left.Value - right.Value);

        public override Object Evaluate(Float left, Float right) => new Float(left.Value - right.Value);

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
            long[] chunks;
            bool isNegative;
            if (left.IsNegative == right.IsNegative)
            {
                var isMoreAbsRight = Greater.Instance.Evaluate(right.Chunks, false, left.Chunks, false);

                chunks = isMoreAbsRight ? Evaluate(right.Chunks, left.Chunks) : Evaluate(left.Chunks, right.Chunks);
                isNegative = left.IsNegative && right.IsNegative ? !isMoreAbsRight : isMoreAbsRight;
            }
            else
            {
                chunks = Addition.Instance.Evaluate(left.Chunks, right.Chunks);
                isNegative = left.IsNegative;
            }

            return Long.Create(chunks, isNegative);
        }

        public long[] Evaluate(ImmutableArray<long> left, ImmutableArray<long> right)
        {
            var dimension = System.Math.Max(left.Length, right.Length);
            var chunks = new long[dimension];

            for (var i = 0; i < dimension; i++)
            {
                var leftChunk = i < left.Length ? left[i] : 0;
                var rightChunk = i < right.Length ? right[i] : 0;

                chunks[i] = leftChunk - rightChunk;
            }

            return chunks;
        }
    }
}
