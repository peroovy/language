using System.Linq;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Multiplication : NumberBinaryOperation
    {
       private Multiplication() { }

        static Multiplication()
        {
            Instance = new Multiplication();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Multiplication;

        public static Multiplication Instance { get; }

        public override Object Evaluate(Int left, Int right) => new Int(left.Value * right.Value);

        public override Object Evaluate(Int left, Float right) => new Float(left.Value * right.Value);

        public override Object Evaluate(Int left, Long right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Float left, Int right) => Evaluate(right, left);

        public override Object Evaluate(Float left, Float right) => new Float(left.Value * right.Value);

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
            var isNegative = left.IsNegative ^ right.IsNegative;
            var chunks = Evaluate(left.Chunks.ToArray(), right.Chunks.ToArray());

            return Long.Create(chunks, isNegative);
        }

        public long[] Evaluate(long[] left, long[] right)
        {
            var maxDimension = System.Math.Max(left.Length, right.Length);
            if (maxDimension <= Long.MediumDimension)
                return MultiplyQuadratically(left, right);

            var dimension = maxDimension + maxDimension % 2;
            var halfDimension = dimension / 2;
            System.Array.Resize(ref left, dimension);
            System.Array.Resize(ref right, dimension);

            var chunks = new long[2 * dimension];

            var xl = left.Take(halfDimension).ToArray();
            var xh = left.Skip(halfDimension).ToArray();
            var yl = right.Take(halfDimension).ToArray();
            var yh = right.Skip(halfDimension).ToArray();

            var xlh = new long[halfDimension];
            var ylh = new long[halfDimension];
            for (var i = 0; i < halfDimension; i++)
            {
                xlh[i] = xl[i] + xh[i];
                ylh[i] = yl[i] + yh[i];
            }

            var ph = Evaluate(xh, yh);
            var pl = Evaluate(xl, yl);
            var plh = Evaluate(xlh, ylh);

            for (var i = 0; i < pl.Length; i++)
                chunks[i] = pl[i];

            for (var i = dimension; i < chunks.Length; i++)
                chunks[i] = ph[i - dimension];

            for (var i = halfDimension; i < plh.Length + halfDimension; i++)
                chunks[i] += plh[i - halfDimension] - pl[i - halfDimension] - ph[i - halfDimension];

            return chunks;
        }

        private static long[] MultiplyQuadratically(long[] left, long[] right)
        {
            var dimension = left.Length + right.Length;
            var chunks = new long[dimension];

            for (var i = 0; i < left.Length; i++)
            {
                for (var j = 0; j < right.Length; j++)
                    chunks[i + j] += left[i] * right[j];
            }

            return chunks;
        }
    }
}
