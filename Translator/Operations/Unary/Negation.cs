using Translator.ObjectModel;
using System.Linq;

namespace Translator
{
    internal sealed class Negation : UnaryOperation
    {
        private Negation() { }

        static Negation()
        {
            Instance = new Negation();
        }

        public override UnaryOperationKind Kind => UnaryOperationKind.Negation;

        public static Negation Instance { get; }

        public override bool IsApplicable(ObjectTypes operand)
        {
            return operand == ObjectTypes.Int
                || operand == ObjectTypes.Float
                || operand == ObjectTypes.Long;
        }

        public override Object Evaluate(Int operand) => new Int(-operand.Value);

        public override Object Evaluate(Float operand) => new Float(-operand.Value);

        public override Object Evaluate(Long operand) => Long.Create(operand.Chunks.ToArray(), !operand.IsNegative);
    }
}
