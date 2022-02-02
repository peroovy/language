using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalOr : IBinaryOperation
    {
        private LogicalOr() { }

        static LogicalOr()
        {
            Instance = new LogicalOr();
        }

        public BinaryOperationKind Kind => BinaryOperationKind.LogicalOr;

        public static LogicalOr Instance { get; }

        public Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Bool && right.Type == ObjectTypes.Bool)
                return (Bool)left | (Bool)right;

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public bool IsApplicable(ObjectTypes left, ObjectTypes right) =>
            left == ObjectTypes.Bool && right == ObjectTypes.Bool;
    }
}
