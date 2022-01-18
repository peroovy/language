using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalOr : IBinaryOperation
    {
        public BinaryOperationKind Kind => BinaryOperationKind.LogicalOr;

        public Object Evaluate(Object left, Object right)
        {
            if (left.Kind == ObjectTypes.Bool && right.Kind == ObjectTypes.Bool)
                return new Bool((left as Bool).Value || (right as Bool).Value);

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public bool IsApplicable(ObjectTypes left, ObjectTypes right) =>
            left == ObjectTypes.Bool && right == ObjectTypes.Bool;
    }
}
