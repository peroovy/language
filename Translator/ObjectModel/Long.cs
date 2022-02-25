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
        }

        public Long(Long value)
        {
            Chunks = value.Chunks.ToImmutableArray();
            IsNegative = value.IsNegative;
        }

        private Long(ImmutableArray<long> chunks, bool isNegative)
        {
            Chunks = chunks;
            IsNegative = isNegative;
        }

        public static readonly int ChunkLength = 5;
        public static readonly int Base = (int)Math.Pow(10, ChunkLength);
        public static readonly int MediumDimension = 2;

        public override ObjectTypes Type => ObjectTypes.Long;
        public string Value => ToString();

        public bool IsNegative { get; }
        public ImmutableArray<long> Chunks { get; }
        public int Dimension => Chunks.Length;

        public Long Absolute => new Long(Chunks.ToImmutableArray(), false);

        public bool IsZero() => Chunks.All(chunk => chunk == 0);

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(IsNegative ? "-" : "");
            builder.Append(Chunks.Last());

            foreach (var chunk in Chunks.Reverse().Skip(1))
                builder.Append(chunk.ToString().PadLeft(ChunkLength, '0'));

            return builder.ToString();
        }

        public Long Take(int counts)
        {
            var chunks = Chunks
                .Skip(Dimension - counts)
                .ToArray();

            return Create(chunks, IsNegative);
        }

        public Long PushBack(long digit)
        {
            if (digit >= Base)
                throw new ArgumentOutOfRangeException();

            var chunks = new long[Dimension + 1];
            chunks[0] = digit;
            Chunks.CopyTo(chunks, 1);

            return Create(chunks, IsNegative);
        }

        public static Long Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException();

            var chunks = new long[value.Length / ChunkLength + 1];
            var isNegative = value[0] == '-';
            var start = isNegative ? 1 : 0;

            for (int i = value.Length, j = 0; i > start; i -= ChunkLength, j++)
            {
                var chunk = i >= ChunkLength
                    ? value.Substring(i - ChunkLength, ChunkLength)
                    : value.Substring(start, i - start);

                chunks[j] = Math.Abs(int.Parse(chunk));
            }

            return Create(chunks, isNegative);
        }

        public static Long Create(int value)
        {
            var absolute = Math.Abs(value);
            var chunks = new long[absolute / Base + 1];

            for (var i = 0; i < chunks.Length && absolute > 0; i++)
            {
                chunks[i] = absolute % Base;
                absolute /= Base;
            }

            return Create(chunks, value < 0);
        }

        public static Long Create(long[] chunks, bool isNegative)
        {
            Normalize(ref chunks);

            if (chunks.Length == 0 || chunks.Length == 1 && chunks[0] == 0)
                return new Long();

            return new Long(chunks.ToImmutableArray(), isNegative);
        }

        public static Long Create(Long obj, bool isNegative) => new Long(obj.Chunks.ToImmutableArray(), isNegative);

        private static void Normalize(ref long[] chunks)
        {
            var normalized = new long[chunks.Length + 1];
            chunks.CopyTo(normalized, 0);

            for (var i = 0; i < normalized.Length - 1; i++)
            {
                if (normalized[i] >= 0 && normalized[i] < Base)
                    continue;

                var carry = normalized[i] >= Base
                    ? normalized[i] / Base
                    : (normalized[i] + 1) / Base - 1;

                normalized[i + 1] += carry;
                normalized[i] -= carry * Base;
            }

            chunks = normalized;

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
    }
}