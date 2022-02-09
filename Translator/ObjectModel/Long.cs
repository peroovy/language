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

        public Long(Long value)
        {
            Chunks = value.Chunks.ToImmutableArray();
            IsNegative = value.IsNegative;
        }

        private Long(ImmutableArray<long> chunks, bool isNegative, string value)
        {
            Chunks = chunks;
            IsNegative = isNegative;
            Value = value;
        }

        public static readonly int ChunkLength = 5;
        public static readonly int Base = (int)Math.Pow(10, ChunkLength);
        public static readonly int MediumDimension = 2;

        public override ObjectTypes Type => ObjectTypes.Long;

        public bool IsNegative { get; }
        public ImmutableArray<long> Chunks { get; }
        public string Value { get; }
        public int Dimension => Chunks.Length;

        public override string ToString() => Value;

        public static Long Create(string value)
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
            if (chunks.Length == 0 || chunks.Length == 1 && chunks[0] == 0)
                return new Long();

            Normalize(ref chunks);
            var builder = new StringBuilder();

            builder.Append(isNegative ? "-" : "");
            builder.Append(chunks.Last());

            foreach (var chunk in chunks.Reverse().Skip(1))
                builder.Append(chunk.ToString().PadLeft(ChunkLength, '0'));

            return new Long(chunks.ToImmutableArray(), isNegative, builder.ToString());
        }

        private static void Normalize(ref long[] chunks)
        {
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                if (chunks[i] >= 0 && chunks[i] < Base)
                    continue;

                var carry = chunks[i] >= Base
                    ? chunks[i] / Base
                    : (chunks[i] + 1) / Base - 1;

                chunks[i + 1] += carry;
                chunks[i] -= carry * Base;
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
    }
}