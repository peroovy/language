using NUnit.Framework;
using System;
using System.Linq;
using System.Numerics;
using Translator.ObjectModel;

namespace Translator.Tests.ObjectModel
{
    [TestFixture]
    internal class LongTests
    {
        private readonly Random _randGenerator = new Random();

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [Repeat(50)]
        public void Create_FromString(int length)
        {
            var num = string.Join("", GenerateDigits(length).Reverse());
            var negative_s = "-" + num;
            var positive = Long.Parse(num).ToString();
            var negitive = Long.Parse(negative_s).ToString();

            Assert.AreEqual(num, positive, num);
            Assert.AreEqual(negative_s, negitive, negative_s);
        }

        [Test]
        public void Create_FromInt32()
        {
            for (var i = 1; i < int.MaxValue / 2; i *= 2)
            {
                var positive = Long.Create(i);
                var negative = Long.Create(-i);

                Assert.AreEqual(positive.ToString(), i.ToString());
                Assert.AreEqual(negative.ToString(), (-i).ToString());
            }
        }

        [TestCase("0000000000000000000000000000000000000000000000000000000000000000000000", "0")]
        [TestCase("-0000000000000000000000000000000000000000000000000000000000000000000000", "0")]
        [TestCase("01", "1")]
        [TestCase("0100", "100")]
        [TestCase("001", "1")]
        [TestCase("00000001", "1")]
        [TestCase("000000001", "1")]
        [TestCase("0000000001", "1")]
        [TestCase("000000000000000000001", "1")]
        [TestCase("00000000000000000000000001", "1")]
        [TestCase("00000000000000000000000000000001", "1")]
        [TestCase("-00000001", "-1")]
        [TestCase("-000000001", "-1")]
        [TestCase("-0000000001", "-1")]
        [TestCase("-000000000000000000001", "-1")]
        [TestCase("-00000000000000000000000001", "-1")]
        [TestCase("-00000000000000000000000000000001", "-1")]
        [TestCase("0000000012345678900876345342346453645656867864567432345111111111111111111111111111111111111111111111111111111111111111111111111", 
            "12345678900876345342346453645656867864567432345111111111111111111111111111111111111111111111111111111111111111111111111")]
        [TestCase("-0000000012345678900876345342346453645656867864567432345111111111111111111111111111111111111111111111111111111111111111111111111", 
            "-12345678900876345342346453645656867864567432345111111111111111111111111111111111111111111111111111111111111111111111111")]
        public void ToString_LeadingZero(string value, string expected = null)
        {
            Check_ToString(value, expected);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void ToString(int start, int end)
        {
            for (var length = start; length <= end; length++)
            {
                var digits = GenerateDigits(length);
                Check_ToString(string.Join("", digits.Reverse()));
                Check_ToString(string.Join("", digits.Reverse()));
            }
        }

        [TestCase("0", "0")]
        [TestCase("-0", "0")]
        public void ToString_SpecialCases(string value, string expected)
        {
            Check_ToString(value, expected);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Addition(int start, int end)
        {
            CheckEvaluation('+', Translator.Addition.Instance, (l, r) => l + r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Subtraction(int start, int end)
        {
            CheckEvaluation('-', Translator.Subtraction.Instance, (l, r) => l - r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Multiplication(int start, int end)
        {
            CheckEvaluation('*', Translator.Multiplication.Instance, (l, r) => l * r, start, end);
        }

        [Test]
        public void ImplicitCasting_FromInt()
        {
            var zero = (Long)ImplicitCasting.Instance.Apply(new Int(), ObjectTypes.Long);
            Assert.AreEqual("0", zero.Value);

            for (var i = 1; i < int.MaxValue - 10; i = i * 2 + i % 3)
            {
                var positive = (Long)ImplicitCasting.Instance.Apply(new Int(i), ObjectTypes.Long);
                var negative = (Long)ImplicitCasting.Instance.Apply(new Int(-i), ObjectTypes.Long);

                Assert.AreEqual(i.ToString(), positive.Value);
                Assert.AreEqual((-i).ToString(), negative.Value);
            }
        }

        private void CheckEvaluation(
            char sign,
            BinaryOperation operation,
            Func<BigInteger, BigInteger, BigInteger> expectedOperation,
            int startLength, int endLength)
        {
            var digits = GenerateDigits(endLength);

            for (var length = startLength; length <= endLength; length++)
            {
                var left_s = string.Join("", digits.Take(length));
                var right_s = string.Join("", digits.Reverse().Take(length));

                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        var signedLeft = (i == 0 ? "-" : "") + left_s;
                        var signedRight = (j == 0 ? "-" : "") + right_s;

                        var left = Long.Parse(signedLeft);
                        var right = Long.Parse(signedRight);

                        var expected = expectedOperation(BigInteger.Parse(signedLeft), BigInteger.Parse(signedRight)).ToString();
                        var actual = operation.Evaluate(left, right).ToString();

                        Assert.AreEqual(expected, actual, $"{signedLeft} {sign} {signedRight}");
                    }
                }
            }
        }

        private short[] GenerateDigits(int length)
        {
            var digits = new short[length];

            for (var i = 0; i < length; i++)
            {
                var digit = (short)_randGenerator.Next(i == length - 1 ? 1 : 0, 10);
                digits[i] = digit;
            }

            return digits;
        }

        private void Check_ToString(string value, string expected = null)
        {
            var actual = Long.Parse(value).ToString();

            Assert.AreEqual(expected ?? value, actual);
        }
    }
}
