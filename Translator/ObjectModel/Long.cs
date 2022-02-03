using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Translator.ObjectModel
{
    internal sealed class Long : Object
    {
        public Long()
        {
            Chunks = ImmutableArray.Create(0L);
            Value = "0";
        }

        private Long(ImmutableArray<long> chunks, bool isNegative, string value)
        {
            Chunks = chunks;
            IsNegative = isNegative;
            Value = value;
        }

        private static readonly int _chunkLength = 5;
        private static readonly int _base = (int)Math.Pow(10, _chunkLength);
        private static readonly int _mediumDimension = 2;

        public override ObjectTypes Type => ObjectTypes.Long;
        public string Value { get; }

        private bool IsNegative { get; }
        private ImmutableArray<long> Chunks { get; }
        private int Dimension => Chunks.Length;

        public override string ToString() => Value;

        public static Long Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException();

            var chunks = new long[value.Length / _chunkLength + 1];
            var isNegative = value[0] == '-';
            var start = isNegative ? 1 : 0;

            for (int i = value.Length, j = 0; i > start; i -= _chunkLength, j++)
            {
                var chunk = i >= _chunkLength
                    ? value.Substring(i - _chunkLength, _chunkLength)
                    : value.Substring(start, i - start);

                chunks[j] = Math.Abs(int.Parse(chunk));
            }

            Normalize(ref chunks);
            return Create(chunks, isNegative);
        }

        public static Long Create(int value)
        {
            var absolute = Math.Abs(value);
            var chunks = new long[absolute / _base + 1];

            for (var i = 0; i < chunks.Length && absolute > 0; i++)
            {
                chunks[i] = absolute % _base;
                absolute /= _base;
            }

            RemoveLeadingZeros(ref chunks);
            return Create(chunks, value < 0);
        }

        private static Long Create(long[] chunks, bool isNegative)
        {
            if (chunks.Length == 0 || chunks.Length == 1 && chunks[0] == 0)
                return new Long();

            var builder = new StringBuilder();

            builder.Append(isNegative ? "-" : "");
            builder.Append(chunks.Last());

            foreach (var chunk in chunks.Reverse().Skip(1))
                builder.Append(chunk.ToString().PadLeft(_chunkLength, '0'));

            return new Long(chunks.ToImmutableArray(), isNegative, builder.ToString());
        }

        private static void Normalize(ref long[] chunks)
        {
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                if (chunks[i] >= 0 && chunks[i] < _base)
                    continue;

                var carry = chunks[i] >= _base
                    ? chunks[i] / _base
                    : (chunks[i] + 1) / _base - 1;

                chunks[i + 1] += carry;
                chunks[i] -= carry * _base;
            }

            RemoveLeadingZeros(ref chunks);
        }

        private static void RemoveLeadingZeros(ref long[] chunks)
        {
            var length = chunks
                .Reverse()
                .TakeWhile(chunk => chunk == 0)
                .Count();

            Array.Resize(ref chunks, chunks.Length - length);
        }

        #region Operation

        private static long[] Sum(long[] left, long[] right)
        {
            var dimension = Math.Max(left.Length, right.Length);
            var chunks = new long[dimension + 1];

            for (var i = 0; i < dimension; i++)
            {
                var leftChunk = i < left.Length ? left[i] : 0;
                var rightChunk = i < right.Length ? right[i] : 0;

                chunks[i] = leftChunk + rightChunk;
            }

            return chunks;
        }

        private static long[] Subtract(long[] left, long[] right)
        {
            var dimension = Math.Max(left.Length, right.Length);
            var chunks = new long[dimension];

            for (var i = 0; i < dimension; i++)
            {
                var leftChunk = i < left.Length ? left[i] : 0;
                var rightChunk = i < right.Length ? right[i] : 0;

                chunks[i] = leftChunk - rightChunk;
            }

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

        private static long[] Multiply(long[] left, long[] right)
        {
            var maxDimension = Math.Max(left.Length, right.Length);
            if (maxDimension <= _mediumDimension)
                return MultiplyQuadratically(left, right);

            var dimension = maxDimension + maxDimension % 2;
            var halfDimension = dimension / 2;
            Array.Resize(ref left, dimension);
            Array.Resize(ref right, dimension);

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

            var ph = Multiply(xh, yh);
            var pl = Multiply(xl, yl);
            var plh = Multiply(xlh, ylh);

            for (var i = 0; i < pl.Length; i++)
                chunks[i] = pl[i];

            for (var i = dimension; i < chunks.Length; i++)
                chunks[i] = ph[i - dimension];
            
            for (var i = halfDimension; i < plh.Length + halfDimension; i++)
                chunks[i] += plh[i - halfDimension] - pl[i - halfDimension] - ph[i - halfDimension];

            return chunks;
        }

        private static bool More(long[] left, bool isNegativeLeft, long[] right, bool isNegativeRight)
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

        #endregion

        #region UnaryOperators

        public static Long operator +(Long operand) => Create(operand.Chunks.ToArray(), operand.IsNegative);

        public static Long operator -(Long operand) => Create(operand.Chunks.ToArray(), !operand.IsNegative);

        #endregion

        #region AdditionOperators

        public static Long operator +(Long left, Int right)
        {
            var longRight = Create(right.Value);

            return left + longRight;
        }

        public static Long operator +(Long left, Long right)
        {
            var leftChunks = left.Chunks.ToArray();
            var rightChunks = right.Chunks.ToArray();
            var isNegativeLeft = left.IsNegative;
            var isNegativeRight = right.IsNegative;

            long[] chunks;
            bool isNegative;
            if (isNegativeLeft == isNegativeRight)
            {
                chunks = Sum(leftChunks, rightChunks);
                isNegative = isNegativeLeft;
            }
            else
            {
                var isMoreAbsRight = More(rightChunks, false, leftChunks, false);

                chunks = isMoreAbsRight ? Subtract(rightChunks, leftChunks) : Subtract(leftChunks, rightChunks);
                isNegative = isMoreAbsRight ? isNegativeRight : isNegativeLeft;
            }

            Normalize(ref chunks);
            return Create(chunks, isNegative);
        }

        #endregion

        #region SubtractionOperators

        public static Long operator -(Long left, Int right)
        {
            var longRight = Create(right.Value);

            return left - longRight;
        }

        public static Long operator -(Long left, Long right)
        {
            var leftChunks = left.Chunks.ToArray();
            var rightChunks = right.Chunks.ToArray();
            var isNegativeLeft = left.IsNegative;
            var isNegativeRight = right.IsNegative;

            long[] chunks;
            bool isNegative;
            if (isNegativeLeft == isNegativeRight)
            {
                var isMoreAbsRight = More(rightChunks, false, leftChunks, false);

                chunks = isMoreAbsRight ? Subtract(rightChunks, leftChunks) : Subtract(leftChunks, rightChunks);
                isNegative = isNegativeLeft && isNegativeRight ? !isMoreAbsRight : isMoreAbsRight;
            }
            else
            {
                chunks = Sum(leftChunks, rightChunks);
                isNegative = isNegativeLeft;
            }


            Normalize(ref chunks);
            return Create(chunks, isNegative);
        }

        #endregion

        #region MultiplicationOperators

        public static Long operator *(Long left, Int right)
        {
            var longRight = Create(right.Value);

            return left * longRight;
        }
        
        public static Long operator *(Long left, Long right)
        {
            var isNegative = left.IsNegative ^ right.IsNegative;
            var chunks = Multiply(left.Chunks.ToArray(), right.Chunks.ToArray());

            Normalize(ref chunks);
            return Create(chunks, isNegative);
        }

        #endregion

        #region LogicalOperators

        public static Bool operator ==(Long left, Long right)
        {
            if (left.IsNegative != right.IsNegative || left.Dimension != right.Dimension)
                return new Bool(false);

            for (var i = left.Dimension - 1; i >= 0; i--)
            {
                if (left.Chunks[i] != right.Chunks[i])
                    return new Bool(false);
            }

            return new Bool(true);
        }

        public static Bool operator !=(Long left, Long right) => !(left == right);

        public static Bool operator >(Long left, Long right)
        {
            if (left.IsNegative != right.IsNegative)
                return new Bool(left.IsNegative == false);

            if (left.Dimension != right.Dimension)
                return new Bool(left.Dimension > right.Dimension);

            for (var i = left.Dimension - 1; i >= 0; i--)
            {
                if (left.Chunks[i] != right.Chunks[i])
                    return new Bool(left.Chunks[i] > right.Chunks[i] ^ left.IsNegative);
            }

            return new Bool(true);
        }

        public static Bool operator >=(Long left, Long right) => left > right | left == right;

        public static Bool operator <(Long left, Long right) => !(left >= right);

        public static Bool operator <=(Long left, Long right) => !(left > right);

        #endregion
    }
}