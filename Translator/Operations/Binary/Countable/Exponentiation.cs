using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Exponentiation : NumberBinaryOperation
    {
        private Exponentiation() { }

        static Exponentiation()
        {
            Instance = new Exponentiation(); 
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Exponentiation;

        public static Exponentiation Instance { get; }

        public override Object Evaluate(Int left, Int right) => new Int(Evaluate(left.Value, right.Value));

        public override Object Evaluate(Int left, Float right) => new Float(Evaluate(left.Value, right.Value));

        public override Object Evaluate(Int left, Long right)
        {
            var leftLong = ImplicitCasting.Instance.Apply(left, ObjectTypes.Long);

            return Evaluate(leftLong, right);
        }

        public override Object Evaluate(Float left, Int right) => new Float(Evaluate(left.Value, right.Value));

        public override Object Evaluate(Float left, Float right) => new Float(Evaluate(left.Value, right.Value));

        public override Object Evaluate(Float left, Long right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Int right)
        {
            var longRight = ImplicitCasting.Instance.Apply(right, ObjectTypes.Long);

            return Evaluate(left, longRight);
        }

        public override Object Evaluate(Long left, Float right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Long right)
        {
            var result = Long.One;
            var isNegative = false;

            var leftAbs = left.Absolute;
            var rightAbs = right.Absolute;
            var two = Long.Create(2);

            while (!rightAbs.IsZero)
            {
                if (rightAbs.IsEven)
                    result = (Long)Multiplication.Instance.Evaluate(result, leftAbs);

                leftAbs = (Long)Multiplication.Instance.Evaluate(leftAbs, leftAbs);
                rightAbs = (Long)Division.Instance.Evaluate(rightAbs, two);
            }

            if (left.IsNegative && right.IsEven)
                isNegative = true;

            if (right.IsNegative)
                result = Division.Instance.Evaluate(Long.One, result, out var remainder);

            return Long.Create(result, isNegative);
        }

        public double Evaluate(double left, double right) => System.Math.Pow(left, right);
    }
}
