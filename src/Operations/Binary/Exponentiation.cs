using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Exponentiation : NumberBinaryOperation
    {
        public override BinaryOperationKind Kind => BinaryOperationKind.Exponentiation;

        public override Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Int)
                return new Int(System.Math.Pow((left as Int).Value, (right as Int).Value));

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Float)
                return new Float(System.Math.Pow((left as Int).Value, (right as Float).Value));

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Int)
                return new Float(System.Math.Pow((left as Float).Value, (right as Int).Value));

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Float)
                return new Float(System.Math.Pow((left as Float).Value, (right as Float).Value));
         
            throw new System.InvalidOperationException();
        }
    }
}
