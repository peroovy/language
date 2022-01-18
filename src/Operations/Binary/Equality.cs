using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Equality : IBinaryOperation
    {
        public BinaryOperationKind Kind => BinaryOperationKind.Equality;

        public Object Evaluate(Object left, Object right)
        { 
            if (left.Kind == ObjectTypes.Object && right.Kind == ObjectTypes.Object)
                return new Bool(left == right);

            if (left.Kind == ObjectTypes.Int && right.Kind == ObjectTypes.Int)
                return (Int)left == (Int)right;

            if (left.Kind == ObjectTypes.Float && right.Kind == ObjectTypes.Float)
                return (Float)left == (Float)right;

            if (left.Kind == ObjectTypes.Bool && right.Kind == ObjectTypes.Bool)
                return (Bool)left == (Bool)right;

            if (left.Kind == ObjectTypes.Null && right.Kind == ObjectTypes.Null)
                return new Bool(true);

            if (left.Kind == ObjectTypes.Int && right.Kind == ObjectTypes.Float)
                return (Int)left == (Float)right;

            if (left.Kind == ObjectTypes.Float && right.Kind == ObjectTypes.Int)
                return (Float)left == (Int)right;
            
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
