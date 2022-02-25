using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Division : NumberBinaryOperation
    {
        private Division() { }

        static Division()
        {
            Instance = new Division();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Division;

        public static Division Instance { get; }

        public override Object Evaluate(Int left, Int right) => right.Value == 0 ? null : new Int(left.Value / right.Value);

        public override Object Evaluate(Int left, Float right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

        public override Object Evaluate(Int left, Long right)
        {
            var longLeft = (Long)ImplicitCasting.Instance.Apply(left, ObjectTypes.Long);

            return Evaluate(longLeft, right);
        }

        public override Object Evaluate(Float left, Int right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

        public override Object Evaluate(Float left, Float right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

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

        public override Object Evaluate(Long left, Long right) => 
            Evaluate(left, right, out var remainder);
            
        public Long Evaluate(Long left, Long right, out Long remainder)
        {
            var isNegative = left.IsNegative ^ right.IsNegative;
            var dividend = left.Absolute;
            var divisor = right.Absolute;
            
            if (right.IsZero)
            {
                remainder = null;
                return null;
            }

            if ((Less.Instance.Evaluate(dividend, divisor) as Bool).Value)
            {
                remainder = dividend;
                return new Long();
            }

            var dimension = dividend.Dimension - divisor.Dimension + 1;
            remainder = dividend.Take(divisor.Dimension);
            if ((Less.Instance.Evaluate(remainder, divisor) as Bool).Value)
            {
                remainder = dividend.Take(divisor.Dimension + 1);
                dimension--;
            }

            var quotient = new long[dimension];
            for (var i = quotient.Length - 1; i >= 0; i--)
            {
                if (i != quotient.Length - 1)
                    remainder = remainder.PushBack(dividend.Chunks[i]);

                var digit = GetNextDigit(remainder, divisor);
                quotient[i] = digit;

                Long mult = Multiplication.Instance.Evaluate(divisor, digit);
                remainder = (Long)Subtraction.Instance.Evaluate(remainder, mult);
            }

            return Long.Create(quotient, isNegative);
        }

        private long GetNextDigit(Long remainder, Long divisor)
        {
            var left = 0;
            var right = Long.Base;

            while (left < right - 1)
            {
                var mid = left + (right - left) / 2;

                Long mult = Multiplication.Instance.Evaluate(divisor, mid);
                if ((LessOrEquality.Instance.Evaluate(mult, remainder) as Bool).Value)
                {
                    left = mid;
                }
                else
                {
                    right = mid;
                }
            }

            return left;
        }
    }
}
