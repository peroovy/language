using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalAnd : BinaryOperation
    {
        private LogicalAnd() { }

        static LogicalAnd()
        {
            Instance = new LogicalAnd();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.LogicalAnd;

        public static LogicalAnd Instance { get; }

        public override ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public override bool IsApplicable(ObjectTypes left, ObjectTypes right) => left == ObjectTypes.Bool && right == ObjectTypes.Bool;

        public override Object Evaluate(Bool left, Bool right) => new Bool(left.Value && right.Value);
    }
}
