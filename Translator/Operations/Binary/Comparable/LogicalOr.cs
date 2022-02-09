using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalOr : BinaryOperation
    {
        private LogicalOr() { }

        static LogicalOr()
        {
            Instance = new LogicalOr();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.LogicalOr;

        public static LogicalOr Instance { get; }

        public override ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public override bool IsApplicable(ObjectTypes left, ObjectTypes right) => left == ObjectTypes.Bool && right == ObjectTypes.Bool;

        public override Object Evaluate(Bool left, Bool right) => new Bool(left.Value || right.Value);
    }
}
