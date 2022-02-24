using System.Collections.Immutable;
using System.Linq;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Addition : NumberBinaryOperation
    {
        private Addition() { }

        static Addition()
        {
            Instance = new Addition();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Addition;
        
        public static Addition Instance { get; }

        public override Object Evaluate(Int left, Int right) => new Int(left.Value + right.Value);

        public override Object Evaluate(Int left, Float right) => new Float(left.Value + right.Value);

        public override Object Evaluate(Int left, Long right) => Evaluate(right, left);

        public override Object Evaluate(Float left, Int right) => Evaluate(right, left);

        public override Object Evaluate(Float left, Float right) => new Float(left.Value + right.Value);

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
                chunks = Evaluate(left.Chunks, right.Chunks);
                isNegative = left.IsNegative;
            }
            else
            {
                var isMoreAbsRight = Greater.Instance.Evaluate(right.Chunks, false, left.Chunks, false);

                chunks = isMoreAbsRight 
                    ? Subtraction.Instance.Evaluate(right.Chunks, left.Chunks) 
                    : Subtraction.Instance.Evaluate(left.Chunks, right.Chunks);

                isNegative = isMoreAbsRight ? right.IsNegative : left.IsNegative;
            }

            return Long.Create(chunks, isNegative);
        }

        public long[] Evaluate(ImmutableArray<long> left, ImmutableArray<long> right)
        {
            var dimension = System.Math.Max(left.Length, right.Length);
            var chunks = new long[dimension + 1];

            for (var i = 0; i < dimension; i++)
            {
                var leftChunk = i < left.Length ? left[i] : 0;
                var rightChunk = i < right.Length ? right[i] : 0;

                chunks[i] = leftChunk + rightChunk;
            }

            return chunks;
        }
    }
}
