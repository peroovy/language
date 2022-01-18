using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Negation : IUnaryOperation
    {
        public UnaryOperationKind Kind => UnaryOperationKind.Negation;

        public Object Evaluate(Object operand)
        {
            if (operand.Kind == ObjectTypes.Int)
                return new Int(-(operand as Int).Value);

            if (operand.Kind == ObjectTypes.Float)
                return new Float(-(operand as Float).Value);

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetResultObjectType(ObjectTypes operand) => operand;

        public bool IsApplicable(ObjectTypes operand)
        {
            return operand == ObjectTypes.Int
                || operand == ObjectTypes.Float;
        }
    }
}
