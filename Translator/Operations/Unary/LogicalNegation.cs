using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalNegation : UnaryOperation
    {
        private LogicalNegation() { }

        static LogicalNegation()
        {
            Instance = new LogicalNegation();
        }

        public override UnaryOperationKind Kind => UnaryOperationKind.LogicalNegation;

        public static LogicalNegation Instance { get; }

        public override bool IsApplicable(ObjectTypes operand) => operand == ObjectTypes.Bool;

        public override Object Evaluate(Bool operand) => new Bool(!operand.Value);
    }
}
