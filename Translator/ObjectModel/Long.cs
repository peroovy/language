using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Translator.ObjectModel
{
    internal sealed class Long : Object
    {
        public Long()
        {
            Chunks = ImmutableArray.Create<uint>(0);
        }

        private Long(ImmutableArray<uint> chunks, bool isNegative)
        {
            Chunks = chunks;
            IsNegative = isNegative;
        }

        public override ObjectTypes Type => ObjectTypes.Long;

        public bool IsNegative { get; }
        public ImmutableArray<uint> Chunks { get; }
        public int Dimension => Chunks.Length;

        public static readonly int ChunkLength = 9;
        public static readonly uint Base = (uint)System.Math.Pow(10, ChunkLength);

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(IsNegative ? "-" : "");
            builder.Append(Chunks.Last());
            foreach (var chunk in Chunks.Reverse().Skip(1))
                builder.Append(chunk.ToString($"D{ChunkLength}"));

            return builder.ToString();
        }

        public static Long Create(uint value, bool isNegative = false)
        {
            var builder = ImmutableArray.CreateBuilder<uint>();

            while (value > 0)
            {
                builder.Add(value % Base);
                value /= Base;
            }

            while (builder.Count > 0 && builder.Last() == 0)
                builder.RemoveAt(builder.Count - 1);

            return new Long(builder.ToImmutableArray(), isNegative);
        }

        public static Long Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new System.InvalidOperationException();

            var builder = ImmutableArray.CreateBuilder<uint>();
            var isNegative = value[0] == '-';
            var start = isNegative ? 1 : 0;

            for (var i = value.Length; i > start; i -= ChunkLength)
            {
                var chunk = i >= ChunkLength
                    ? value.Substring(i - ChunkLength, ChunkLength)
                    : value.Substring(start, i - start);

                builder.Add(uint.Parse(chunk));
            }

            while (builder.Count > 1 && builder.Last() == 0)
                builder.RemoveAt(builder.Count - 1);

            return new Long(builder.ToImmutableArray(), isNegative);
        }

        public static Long Create(uint[] chunks, bool isNegative = false)
        {
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                chunks[i + 1] += chunks[i] / Base;
                chunks[i] %= Base;
            }

            var length = chunks.Length;
            while (length - 1 > 0 && chunks[length - 1] == 0u)
                length -= 1;

            return new Long(chunks.Take(length).ToImmutableArray(), isNegative);
        }

        private static Long Sum(ImmutableArray<uint> left, bool isNegativeLeft, ImmutableArray<uint> right, bool isNegativeRight)
        {
            if (isNegativeLeft == isNegativeRight)
            {
                var isNegative = isNegativeLeft;
                var dimension = System.Math.Max(left.Length, right.Length);
                var chunks = new uint[dimension + 1];

                var carry = 0u;
                for (var i = 0; i < dimension || carry > 0u; i++)
                {
                    var leftChunk = i < left.Length ? left[i] : 0u;
                    var rightChunk = i < right.Length ? right[i] : 0u;

                    chunks[i] += leftChunk + rightChunk + carry;
                    carry = chunks[i] >= Base ? 1u : 0u;
                    chunks[i] -= carry * Base;
                }

                return Create(chunks, isNegative);
            }

            return isNegativeLeft
                ? Sub(right, false, left, false)
                : Sub(left, false, right, false);
        }

        private static Long Sub(ImmutableArray<uint> left, bool isNegativeLeft, ImmutableArray<uint> right, bool isNegativeRight)
        {
            if (!isNegativeLeft && !isNegativeRight)
            {
                var dimension = System.Math.Max(left.Length, right.Length);
                var chunks = new uint[dimension];
                var isNegative = false;

                if (More(right, isNegativeRight, left, isNegativeLeft))
                {
                    var temp = left;
                    left = right;
                    right = temp;
                    isNegative = true;
                }

                var carry = 0u;
                for (var i = 0; i < dimension; i++)
                {
                    var leftChunk = i < left.Length ? left[i] : 0u;
                    var rightChunk = i < right.Length ? right[i] : 0u;
                    var occupied = leftChunk < rightChunk ? Base : 0u;

                    chunks[i] = occupied - rightChunk - carry + leftChunk;
                    carry = leftChunk < rightChunk ? 1u : 0u;
                }

                return Create(chunks, isNegative);
            }

            if (isNegativeLeft && isNegativeRight)
                return Sub(right, false, left, false);

            return isNegativeLeft
                ? Sum(left, true, right, true)
                : Sum(left, false, right, false);
        }

        private static bool More(ImmutableArray<uint> left, bool isNegativeLeft, ImmutableArray<uint> right, bool isNegativeRight)
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

        public static Long operator +(Long operand) => new Long(operand.Chunks.ToImmutableArray(), operand.IsNegative);

        public static Long operator -(Long operand) => new Long(operand.Chunks.ToImmutableArray(), !operand.IsNegative);

        #endregion


        #region AdditionOperators

        public static Long operator +(Long left, Int right)
        {
            var longRight = Create((uint)System.Math.Abs(right.Value), right.Value < 0);

            return left + longRight;
        }

        public static Long operator +(Long left, Long right)
        {
            return Sum(left.Chunks, left.IsNegative, right.Chunks, right.IsNegative);
        }

        #endregion

        #region SubtractionOperators

        public static Long operator -(Long left, Int right)
        {
            var longRight = Create((uint)System.Math.Abs(right.Value), right.Value < 0);

            return left - longRight;
        }

        public static Long operator -(Long left, Long right)
        {
            return Sub(left.Chunks, left.IsNegative, right.Chunks, right.IsNegative);
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