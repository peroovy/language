using Translator.ObjectModel;

namespace Translator
{
    internal sealed class NotEquality : IBinaryOperation
    {
        private NotEquality() { }

        static NotEquality()
        {
            Instance = new NotEquality();
        }

        public BinaryOperationKind Kind => BinaryOperationKind.NotEquality;

        public static NotEquality Instance { get; }

        public Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Object && right.Type == ObjectTypes.Object)
                return new Bool(left != right);

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Int)
                return (Int)left != (Int)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Float)
                return (Float)left != (Float)right;

            if (left.Type == ObjectTypes.Bool && right.Type == ObjectTypes.Bool)
                return (Bool)left != (Bool)right;

            if (left.Type == ObjectTypes.Null && right.Type == ObjectTypes.Null)
                return new Bool(false);

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Float)
                return (Int)left != (Float)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Int)
                return (Float)left != (Int)right;

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public bool IsApplicable(ObjectTypes left, ObjectTypes right)
        {
            return left == right
                || left == ObjectTypes.Int && right == ObjectTypes.Float
                || left == ObjectTypes.Float && right == ObjectTypes.Int;
        }
    }
}
