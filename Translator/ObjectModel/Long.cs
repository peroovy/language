using System.Linq;
using System.Text;

namespace Translator.ObjectModel
{
    internal sealed class Long : Object
    {
        private bool _isNegative;
        private long[] _chunks;

        public Long()
        {
            _chunks = new long[] { 0 };
        }

        private Long(long[] chunks, bool isNegative)
        {
            if (chunks.Length == 0 || chunks.Length == 1 && chunks[0] == 0)
            {
                _chunks = new long[] { 0 };
                _isNegative = false;

                return;
            }

            _chunks = chunks;
            _isNegative = isNegative;
        }

        private static readonly int _chunkLength = 5;
        private static readonly int _base = (int)System.Math.Pow(10, _chunkLength);
        private static readonly int _mediumDimension = 2;

        public override ObjectTypes Type => ObjectTypes.Long;
        public string Value => ToString();

        private int Dimension => _chunks.Length;

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(_isNegative ? "-" : "");
            builder.Append(_chunks.Last());
            foreach (var chunk in _chunks.Reverse().Skip(1))
                builder.Append(chunk.ToString($"D{_chunkLength}"));

            return builder.ToString();
        }

        public static Long Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new System.InvalidOperationException();

            var chunks = new long[value.Length / _chunkLength + 1];
            var isNegative = value[0] == '-';
            var start = isNegative ? 1 : 0;

            for (int i = value.Length, j = 0; i > start; i -= _chunkLength, j++)
            {
                var chunk = i >= _chunkLength
                    ? value.Substring(i - _chunkLength, _chunkLength)
                    : value.Substring(start, i - start);

                chunks[j] = System.Math.Abs(int.Parse(chunk));
            }

            Normalize(ref chunks);
            return new Long(chunks, isNegative);
        }

        public static Long Create(int value)
        {
            var absolute = System.Math.Abs(value);
            var chunks = new long[absolute / _base + 1];

            for (var i = 0; i < chunks.Length && absolute > 0; i++)
            {
                chunks[i] = absolute % _base;
                absolute /= _base;
            }

            RemoveLeadingZeros(ref chunks);
            return new Long(chunks, value < 0);
        }

        private static void Normalize(ref long[] chunks)
        {
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                if (chunks[i] >= _base)
                {
                    var carryover = chunks[i] / _base;
                    chunks[i + 1] += carryover;
                    chunks[i] -= carryover * _base;
                }
                else if (chunks[i] < 0)
                {
                    var carryover = (chunks[i] + 1) / _base - 1;
                    chunks[i + 1] += carryover;
                    chunks[i] -= carryover * _base;
                }
            }

            RemoveLeadingZeros(ref chunks);
        }

        private static void RemoveLeadingZeros(ref long[] chunks)
        {
            var length = chunks
                .Reverse()
                .TakeWhile(chunk => chunk == 0)
                .Count();

            System.Array.Resize(ref chunks, chunks.Length - length);
        }

        private static long[] Sum(long[] left, long[] right)
        {
            var dimension = System.Math.Max(left.Length, right.Length);
            var chunks = new long[dimension + 1];

            for (var i = 0; i < dimension; i++)
            {
                var leftChunk = i < left.Length ? left[i] : 0;
                var rightChunk = i < right.Length ? right[i] : 0;

                chunks[i] = leftChunk + rightChunk;
            }

            Normalize(ref chunks);
            return chunks;
        }

        private static long[] Sub(long[] left, long[] right)
        {
            var dimension = System.Math.Max(left.Length, right.Length);
            var chunks = new long[dimension];

            for (var i = 0; i < dimension; i++)
            {
                var leftChunk = i < left.Length ? left[i] : 0;
                var rightChunk = i < right.Length ? right[i] : 0;

                chunks[i] = leftChunk - rightChunk;
            }

            Normalize(ref chunks);
            return chunks;
        }

        private static long[] Mult(long[] left, long[] right)
        {
            var dimension = System.Math.Max(left.Length, right.Length);
            dimension += dimension % 2;

            System.Array.Resize(ref left, dimension);
            System.Array.Resize(ref right, dimension);

            var chunks = new long[2 * dimension];
            var halfDimension = dimension / 2;

            if (dimension <= _mediumDimension)
            {
                for (var i = 0; i < left.Length; i++)
                {
                    for (var j = 0; j < right.Length; j++)
                        chunks[i + j] += left[i] * right[j];
                }

                return chunks;
            }

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

            var ph = Mult(xh, yh);
            var pl = Mult(xl, yl);
            var plh = Mult(xlh, ylh);

            for (var i = 0; i < pl.Length; i++)
                chunks[i] = pl[i];

            for (var i = dimension; i < chunks.Length; i++)
                chunks[i] += ph[i - dimension];
            
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

        #region UnaryOperators

        public static Long operator +(Long operand) => new Long(operand._chunks.ToArray(), operand._isNegative);

        public static Long operator -(Long operand) => new Long(operand._chunks.ToArray(), !operand._isNegative);
        #endregion


        #region AdditionOperators

        public static Long operator +(Long left, Int right)
        {
            var longRight = Create(right.Value);

            return left + longRight;
        }

        public static Long operator +(Long left, Long right)
        {
            var leftChunks = left._chunks;
            var rightChunks = right._chunks;
            var isNegativeLeft = left._isNegative;
            var isNegativeRight = right._isNegative;

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
                chunks = isMoreAbsRight ? Sub(rightChunks, leftChunks) : Sub(leftChunks, rightChunks);
                isNegative = isMoreAbsRight ? isNegativeRight : isNegativeLeft;
            }

            Normalize(ref chunks);
            return new Long(chunks, isNegative);
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
            var leftChunks = left._chunks;
            var rightChunks = right._chunks;
            var isNegativeLeft = left._isNegative;
            var isNegativeRight = right._isNegative;

            long[] chunks;
            bool isNegative;
            var isMoreAbsRight = More(rightChunks, false, leftChunks, false);
            if (!isNegativeLeft && !isNegativeRight)
            {
                chunks = isMoreAbsRight ? Sub(rightChunks, leftChunks) : Sub(leftChunks, rightChunks);
                isNegative = isMoreAbsRight;
            }
            else if (isNegativeLeft && isNegativeRight)
            {
                chunks = isMoreAbsRight ? Sub(rightChunks, leftChunks) : Sub(leftChunks, rightChunks);
                isNegative = !isMoreAbsRight;
            }
            else
            {
                chunks = Sum(leftChunks, rightChunks);
                isNegative = isNegativeLeft;
            }

            return new Long(chunks, isNegative);
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
            var isNegative = left._isNegative ^ right._isNegative;

            var leftChunks = left._chunks.ToArray();
            var rightChunks = right._chunks.ToArray();

            var chunks = Mult(leftChunks, rightChunks);
            Normalize(ref chunks);
            return new Long(chunks, isNegative);
        }

        #endregion

        #region LogicalOperators

        public static Bool operator ==(Long left, Long right)
        {
            if (left._isNegative != right._isNegative || left.Dimension != right.Dimension)
                return new Bool(false);

            for (var i = left.Dimension - 1; i >= 0; i--)
            {
                if (left._chunks[i] != right._chunks[i])
                    return new Bool(false);
            }

            return new Bool(true);
        }

        public static Bool operator !=(Long left, Long right) => !(left == right);

        public static Bool operator >(Long left, Long right)
        {
            if (left._isNegative != right._isNegative)
                return new Bool(left._isNegative == false);

            if (left.Dimension != right.Dimension)
                return new Bool(left.Dimension > right.Dimension);

            for (var i = left.Dimension - 1; i >= 0; i--)
            {
                if (left._chunks[i] != right._chunks[i])
                    return new Bool(left._chunks[i] > right._chunks[i] ^ left._isNegative);
            }

            return new Bool(true);
        }

        public static Bool operator >=(Long left, Long right) => left > right | left == right;

        public static Bool operator <(Long left, Long right) => !(left >= right);

        public static Bool operator <=(Long left, Long right) => !(left > right);

        #endregion
    }
}