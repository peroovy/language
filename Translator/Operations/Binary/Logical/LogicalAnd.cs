using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalAnd : LogicalBinaryOperation
    {
        private LogicalAnd() { }

        static LogicalAnd()
        {
            Instance = new LogicalAnd();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.LogicalAnd;

        public static LogicalAnd Instance { get; }

        public override Object Evaluate(Bool left, Bool right) => new Bool(left.Value && right.Value);
    }
}
