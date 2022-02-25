using NUnit.Framework;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
            CheckEvaluation("+", Translator.Addition.Instance, (l, r) => l + r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Subtraction(int start, int end)
        {
            CheckEvaluation("-", Translator.Subtraction.Instance, (l, r) => l - r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Multiplication(int start, int end)
        {
            CheckEvaluation("*", Translator.Multiplication.Instance, (l, r) => l * r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Division(int start, int end)
        {
            CheckEvaluation("/", Translator.Division.Instance, (l, r) => l / r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Equality(int start, int end)
        {
            CheckEvaluation("==", Translator.Equality.Instance, (l, r) => l == r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void NotEquality(int start, int end)
        {
            CheckEvaluation("!=", Translator.NotEquality.Instance, (l, r) => l != r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Greater(int start, int end)
        {
            CheckEvaluation(">", Translator.Greater.Instance, (l, r) => l > r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void GreaterOrEquality(int start, int end)
        {
            CheckEvaluation(">=", Translator.GreaterOrEquality.Instance, (l, r) => l >= r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void Less(int start, int end)
        {
            CheckEvaluation("<", Translator.Less.Instance, (l, r) => l < r, start, end);
        }

        [TestCase(1, 5)]
        [TestCase(6, 10)]
        [TestCase(11, 100)]
        [TestCase(101, 1000)]
        [TestCase(1001, 2000)]
        public void LessOrEquality(int start, int end)
        {
            CheckEvaluation("<=", Translator.LessOrEquality.Instance, (l, r) => l <= r, start, end);
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

        private void CheckEvaluation<TResultExpected>(
            string sign,
            BinaryOperation operation,
            Func<BigInteger, BigInteger, TResultExpected> expectedOperation,
            int startLength, int endLength)
        {
            var digits = GenerateDigits(endLength);

            for (var leftLength = startLength; leftLength <= endLength; leftLength++)
            {
                var left = string.Join("", digits.Take(leftLength));

                var step = leftLength < 4 ? 1 : leftLength / 4;
                for (var rightLength = step; rightLength <= leftLength; rightLength += step)
                {
                    var right = string.Join("", digits.Take(rightLength));
                    RunOperationCheckerAsync(sign, operation, expectedOperation, left, right);
                }
            }
        }

        private void RunOperationCheckerAsync<TResultExpected>(
            string sign, 
            BinaryOperation operation, 
            Func<BigInteger, BigInteger, TResultExpected> expectedOperation, 
            string left, string right)
        {
            var task = Task.Run((() =>
            {
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        var signedLeft = (i == 0 ? "-" : "") + left;
                        var signedRight = (j == 0 ? "-" : "") + right;

                        var leftLong = Long.Parse(signedLeft);
                        var rightLong = Long.Parse(signedRight);

                        var expected = expectedOperation(
                            BigInteger.Parse(signedLeft), BigInteger.Parse(signedRight)).ToString();
                        var actual = operation.Evaluate(leftLong, rightLong).ToString();

                        Assert.AreEqual(expected.ToLower(), actual.ToLower(), $"{signedLeft} {sign} {signedRight}");
                    }
                }
            }));
        }

        private short[] GenerateDigits(int length)
        {
            var digits = new short[length];

            for (var i = 0; i < length; i++)
            {
                var digit = (short)_randGenerator.Next(1, 10);
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
