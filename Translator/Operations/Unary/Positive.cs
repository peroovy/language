using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Positive : UnaryOperation
    {
        private Positive() { }

        static Positive()
        {
            Instance = new Positive();
        }

        public override UnaryOperationKind Kind => UnaryOperationKind.Negation;

        public static Positive Instance { get; }

        public override bool IsApplicable(ObjectTypes operand)
        {
            return operand == ObjectTypes.Int
                || operand == ObjectTypes.Float
                || operand == ObjectTypes.Long;
        }

        public override Object Evaluate(Int operand) => new Int(operand.Value);

        public override Object Evaluate(Float operand) => new Float(operand.Value);

        public override Object Evaluate(Long operand) => new Long(operand);
    }
}
