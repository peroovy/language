using NUnit.Framework;
using System;
using System.Linq;
using Translator.ObjectModel;

namespace Translator.Tests.ObjectModel
{
    [TestFixture]
    internal class LongTests
    {
        private readonly Random _randGenerator = new Random();

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void Test_CreateFromString(int length)
        {
            var num = string.Join("", GenerateDigits(length).Reverse());
            var negative_s = "-" + num;
            var positive = Long.Create(num).ToString();
            var negitive = Long.Create(negative_s).ToString();

            Assert.AreEqual(num, positive, num);
            Assert.AreEqual(negative_s, negitive, negative_s);
        }

        [TestCase("0")]
        [TestCase("01", "1")]
        [TestCase("001", "1")]
        [TestCase("00000001", "1")]
        [TestCase("000000001", "1")]
        [TestCase("0000000001", "1")]
        [TestCase("0000000000000000000000000000000000000000000000000000000000000000000000", "0")]
        [TestCase("0000000012345678900876345342346453645656867864567432345", "12345678900876345342346453645656867864567432345")]
        [TestCase("01010101010010101010101010101010100101010101010101010", "1010101010010101010101010101010100101010101010101010")]
        public void LeadingZero_ToString(string value, string expected = null)
        {
            Check_ToString(value, expected);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void ToString_Positive(int length)
        {
            var digits = GenerateDigits(length);

            Check_ToString(string.Join("", digits.Reverse()));
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void ToString_Negative(int length)
        {
            var digits = GenerateDigits(length);

            Check_ToString("-" + string.Join("", digits.Reverse()));
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void ToString_Range(int start, int end)
        {
            for (var length = start; length <= end; length++)
            {
                ToString_Positive(length);
                ToString_Negative(length);
            }
        }

        [TestCase("0", "0")]
        [TestCase("-0", "0")]
        public void ToString_SpecialCases(string value, string expected)
        {
            Check_ToString(value, expected);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void Addition_Int(int length)
        {
            CheckBinaryOperation_Long_Int("+", Addition.Instance, Sum, length);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void Addition_Range_Int(int start, int end)
        {
            for (var length = start; length <= end; length++)
                CheckBinaryOperation_Long_Int("+", Addition.Instance, Sum, length);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void Addition_Long(int length)
        {
            CheckBinaryOperation_Long_Long("+", Addition.Instance, Sum, length);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void Addition_Range_Long(int start, int end)
        {
            for (var length = start; length <= end; length++)
                CheckBinaryOperation_Long_Long("+", Addition.Instance, Sum, length);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void Subtraction_Int(int length)
        {
            CheckBinaryOperation_Long_Int("-", Subtraction.Instance, Sub, length);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void Subtraction_Range_Int(int start, int end)
        {
            for (var length = start; length <= end; length++)
                CheckBinaryOperation_Long_Int("-", Subtraction.Instance, Sub, length);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Repeat(100)]
        public void Subtraction_Long(int length)
        {
            CheckBinaryOperation_Long_Long("-", Subtraction.Instance, Sub, length);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void Subtraction_Range_Long(int start, int end)
        {
            for (var length = start; length <= end; length++)
                CheckBinaryOperation_Long_Long("-", Subtraction.Instance, Sub, length);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [Repeat(100)]
        public void Multiplication_Int(int length)
        {
            CheckBinaryOperation_Long_Int("*", Multiplication.Instance, Mult, length);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void Multiplication_Range_Int(int start, int end)
        {
            for (var length = start; length <= end; length++)
                CheckBinaryOperation_Long_Int("*", Multiplication.Instance, Mult, length);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [Repeat(100)]
        public void Multiplication_Long(int length)
        {
            CheckBinaryOperation_Long_Long("*", Multiplication.Instance, Mult, length);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 200)]
        [TestCase(201, 300)]
        [TestCase(301, 400)]
        [TestCase(501, 600)]
        [TestCase(601, 700)]
        public void Multiplication_Range_Long(int start, int end)
        {
            for (var length = start; length <= end; length++)
                CheckBinaryOperation_Long_Long("*", Multiplication.Instance, Mult, length);
        }

        private void CheckBinaryOperation_Long_Int(
            string sign,
            IBinaryOperation operation, 
            Func<int[], bool, int[], bool, string> expectedOperation, 
            int length)
        {
            var leftDigits = GenerateDigits(length);
            for (var rightLength = 1; rightLength <= 9; rightLength++)
            {
                var rightDigits = GenerateDigits(rightLength);
                var left_s = string.Join("", leftDigits.Reverse());
                var right_s = string.Join("", rightDigits.Reverse());

                for (var i = 0; i < 2; i++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var left = (i == 0 ? "-" : "") + left_s;
                        var right = (k == 0 ? "-" : "") + right_s;

                        var leftObj = Long.Create(left);
                        var rightObj = Int.Create(right);

                        var expected = expectedOperation(leftDigits, i == 0, rightDigits, k == 0);
                        var actual = operation.Evaluate(leftObj, rightObj).ToString();
                        Assert.AreEqual(expected, actual, $"{left} {sign} {right}");
                    }
                }
            }
        }

        private void CheckBinaryOperation_Long_Long(
            string sign,
            IBinaryOperation operation,
            Func<int[], bool, int[], bool, string> expectedOperation,
            int length)
        {
            var step = Math.Max(1, length / 4);
            var leftDigits = GenerateDigits(length);
            for (var rightLength = 1; rightLength <= length; rightLength += step)
            {
                var rightDigits = GenerateDigits(rightLength);
                var left_s = string.Join("", leftDigits.Reverse());
                var right_s = string.Join("", rightDigits.Reverse());

                for (var i = 0; i < 2; i++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var left = (i == 0 ? "-" : "") + left_s;
                        var right = (k == 0 ? "-" : "") + right_s;

                        var longLeft = Long.Create(left);
                        var longRight = Long.Create(right);

                        var expected = expectedOperation(leftDigits, i == 0, rightDigits, k == 0);
                        var actual = operation.Evaluate(longLeft, longRight).ToString();
                        Assert.AreEqual(expected, actual, $"{left} {sign} {right}");
                    }
                }
            }
        }

        private string Sum(int[] left, bool isNegativeLeft, int[] right, bool isNegativeRight)
        {
            if (isNegativeLeft == isNegativeRight)
            {
                var length = Math.Max(left.Length, right.Length);
                var sum = new int[length + 1];
                var isNegative = isNegativeLeft;
                var carry = 0;

                for (var i = 0; i < length || carry > 0; i++)
                {
                    var l = i < left.Length ? left[i] : 0;
                    var r = i < right.Length ? right[i] : 0;

                    sum[i] = l + r + carry;
                    carry = sum[i] > 9 ? 1 : 0;
                    sum[i] -= carry * 10;
                }

                var length_s = sum.Length;
                while (length_s - 1 > 0 && sum[length_s - 1] == 0)
                    length_s -= 1;

                if (sum.Length == 1 && sum[0] == 0)
                    isNegative = false;

                return (isNegative ? "-" : "") + string.Join("", sum.Take(length_s).Reverse());
            }

            return isNegativeLeft 
                ? Sub(right, false, left, false)
                : Sub(left, false, right, false);
        }

        private string Sub(int[] left, bool isNegativeLeft, int[] right, bool isNegativeRight)
        {
            if (isNegativeLeft && isNegativeRight)
                return Sub(right, false, left, false);

            if (!isNegativeLeft && !isNegativeRight)
            {
                var length = Math.Max(left.Length, right.Length);
                var digits = new int[length];
                var isNegative = false;

                if (More(right, isNegativeRight, left, isNegativeLeft))
                {
                    var temp = left;
                    left = right;
                    right = temp;
                    isNegative = true;
                }

                var carry = 0;
                for (var i = 0; i < digits.Length; i++)
                {
                    var l = i < left.Length ? left[i] : 0;
                    var r = i < right.Length ? right[i] : 0;

                    digits[i] = l - r - carry;
                    carry = digits[i] < 0 ? 1 : 0;
                    digits[i] += carry * 10;
                }

                var length_s = digits.Length;
                while (length_s - 1 > 0 && digits[length_s - 1] == 0)
                    length_s -= 1;

                if (digits.Length == 1 && digits[0] == 0)
                    isNegative = false;

                return (isNegative ? "-" : "") + string.Join("", digits.Take(length_s).Reverse());
            }

            return isNegativeLeft
                ? Sum(left, true, right, true)
                : Sum(left, false, right, false);
        }

        private string Mult(int[] left, bool isNegativeLeft, int[] right, bool isNegativeRight)
        {
            var length = left.Length + right.Length;
            var digits = new int[length];
            var isNegative = isNegativeLeft ^ isNegativeRight;

            for (var i = 0; i < left.Length; i++)
            {
                for (var j = 0; j < right.Length; j++)
                    digits[i + j] += left[i] * right[j];
            }

            for (var i = 0; i < length - 1; i++)
            {
                digits[i + 1] += digits[i] / 10;
                digits[i] %= 10;
            }

            var length_s = digits.Length;
            while (length_s - 1 > 0 && digits[length_s - 1] == 0)
                length_s -= 1;

            if (digits.Length == 1 && digits[0] == 0)
                isNegative = false;

            return (isNegative ? "-" : "") + string.Join("", digits.Take(length_s).Reverse());
        }

        private bool More(int[] left, bool isNegativeLeft, int[] right, bool isNegativeRight)
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

        private int[] GenerateDigits(int length)
        {
            var digits = new int[length];

            for (var i = 0; i < length; i++)
            {
                var digit = _randGenerator.Next(i == length - 1 ? 1 : 0, 10);
                digits[i] = digit;
            }

            return digits;
        }

        private void Check_ToString(string value, string expected = null)
        {
            var actual = Long.Create(value).ToString();

            Assert.AreEqual(expected ?? value, actual);
        }
    }
}
