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
            var leftChunks = left.Chunks.ToArray();
            var rightChunks = right.Chunks.ToArray();
            var isNegativeLeft = left.IsNegative;
            var isNegativeRight = right.IsNegative;

            long[] chunks;
            bool isNegative;
            if (isNegativeLeft == isNegativeRight)
            {
                chunks = Evaluate(leftChunks, rightChunks);
                isNegative = isNegativeLeft;
            }
            else
            {
                var isMoreAbsRight = Greater.Instance.Evaluate(rightChunks, false, leftChunks, false);

                chunks = isMoreAbsRight 
                    ? Subtraction.Instance.Evaluate(rightChunks, leftChunks) 
                    : Subtraction.Instance.Evaluate(leftChunks, rightChunks);

                isNegative = isMoreAbsRight ? isNegativeRight : isNegativeLeft;
            }

            return Long.Create(chunks, isNegative);
        }

        public long[] Evaluate(long[] left, long[] right)
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
