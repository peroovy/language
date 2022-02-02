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

        public Long(ImmutableArray<uint> digits, bool isNegative) : this(digits)
        {
            IsNegative = isNegative;
        }

        private Long(ImmutableArray<uint> digits)
        {
            Chunks = digits.Length > 0 ? digits : ImmutableArray.Create<uint>(0);
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

            while (builder.Count > 0 && builder.Last() == 0)
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

        public static Long operator +(Long operand) => Create(operand.Chunks.ToArray(), operand.IsNegative);

        public static Long operator -(Long operand) => Create(operand.Chunks.ToArray(), !operand.IsNegative);

        public static Long operator +(Long left, Int right)
        {
            var longRight = Create((uint)System.Math.Abs(right.Value), right.Value < 0);

            return left + longRight;
        }

        public static Long operator +(Long left, Long right)
        {
            if (left.IsNegative == right.IsNegative)
            {
                var isNegative = left.IsNegative;
                var dimension = System.Math.Max(left.Dimension, right.Dimension);
                var chunks = new uint[dimension + 1];

                var carry = 0u;
                for (var i = 0; i < dimension || carry > 0u; i++)
                {
                    var leftChunk = i < left.Chunks.Length ? left.Chunks[i] : 0u;
                    var rightChunk = i < right.Chunks.Length ? right.Chunks[i] : 0u;

                    chunks[i] += leftChunk + rightChunk + carry;
                    carry = chunks[i] >= Base ? 1u : 0u;
                    chunks[i] -= carry * Base;
                }

                return Create(chunks, isNegative);
            }

            return left.IsNegative
                ? right - (-left)
                : left - (-right);
        }

        public static Long operator -(Long left, Int right)
        {
            var longRight = Create((uint)System.Math.Abs(right.Value), right.Value < 0);

            return left - longRight;
        }

        public static Long operator -(Long left, Long right)
        {
            if (!left.IsNegative && !right.IsNegative)
            {
                var dimension = System.Math.Max(left.Dimension, right.Dimension);
                var chunks = new uint[dimension];
                var isNegative = false;

                if ((right > left).Value)
                {
                    var temp = left;
                    left = right;
                    right = temp;
                    isNegative = true;
                }

                var carry = 0u;
                for (var i = 0; i < dimension; i++)
                {
                    var leftChunk = i < left.Chunks.Length ? left.Chunks[i] : 0u;
                    var rightChunk = i < right.Chunks.Length ? right.Chunks[i] : 0u;
                    var occupied = leftChunk < rightChunk ? Base : 0u;

                    chunks[i] = occupied - rightChunk - carry + leftChunk;
                    carry = leftChunk < rightChunk ? 1u : 0u;
                }

                return Create(chunks, isNegative);
            }

            if (left.IsNegative && right.IsNegative)
                return (-right) - (-left);

            return left + (-right);
        }

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

    }
}