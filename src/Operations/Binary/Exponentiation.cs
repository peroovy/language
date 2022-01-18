using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Exponentiation : NumberBinaryOperation
    {
        public override BinaryOperationKind Kind => BinaryOperationKind.Exponentiation;

        public override Object Evaluate(Object left, Object right)
        {
            if (left.Kind == ObjectTypes.Int && right.Kind == ObjectTypes.Int)
                return new Int(System.Math.Pow((left as Int).Value, (right as Int).Value));

            if (left.Kind == ObjectTypes.Int && right.Kind == ObjectTypes.Float)
                return new Float(System.Math.Pow((left as Int).Value, (right as Float).Value));

            if (left.Kind == ObjectTypes.Float && right.Kind == ObjectTypes.Int)
                return new Float(System.Math.Pow((left as Float).Value, (right as Int).Value));

            if (left.Kind == ObjectTypes.Float && right.Kind == ObjectTypes.Float)
                return new Float(System.Math.Pow((left as Float).Value, (right as Float).Value));
         
            throw new System.InvalidOperationException();
        }
    }
}
