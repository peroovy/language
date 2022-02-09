using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalOr : LogicalBinaryOperation
    {
        private LogicalOr() { }

        static LogicalOr()
        {
            Instance = new LogicalOr();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.LogicalOr;

        public static LogicalOr Instance { get; }

        public override Object Evaluate(Bool left, Bool right) => new Bool(left.Value || right.Value);
    }
}
