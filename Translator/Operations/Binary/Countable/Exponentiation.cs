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
            throw new System.NotImplementedException();
        }


        public override Object Evaluate(Float left, Int right) => new Float(Evaluate(left.Value, right.Value));

        public override Object Evaluate(Float left, Float right) => new Float(Evaluate(left.Value, right.Value));

        public override Object Evaluate(Float left, Long right)
        {
            throw new System.NotImplementedException();
        }


        public override Object Evaluate(Long left, Int right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Float right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Long right)
        {
            throw new System.NotImplementedException();
        }

        public double Evaluate(double left, double right) => System.Math.Pow(left, right);
    }
}
