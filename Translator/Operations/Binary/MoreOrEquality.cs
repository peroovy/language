using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class MoreOrEquality : IBinaryOperation
    {
        private readonly Dictionary<(ObjectTypes, ObjectTypes), System.Func<Object, Object, Object>> _evaluations
           = new Dictionary<(ObjectTypes, ObjectTypes), System.Func<Object, Object, Object>>()
           {
               [(ObjectTypes.Int, ObjectTypes.Int)] = (left, right) => (Int)left >= (Int)right,
               [(ObjectTypes.Int, ObjectTypes.Float)] = (left, right) => (Int)left >= (Float)right,
               //[(ObjectTypes.Int, ObjectTypes.Long)] = (left, right) => (Int)left >= (Long)right,

               [(ObjectTypes.Float, ObjectTypes.Int)] = (left, right) => (Float)left >= (Int)right,
               [(ObjectTypes.Float, ObjectTypes.Float)] = (left, right) => (Float)left >= (Float)right,
               //[(ObjectTypes.Float, ObjectTypes.Long)] = (left, right) => (Float)left >= (Long)right,

               //[(ObjectTypes.Long, ObjectTypes.Int)] = (left, right) => (Long)left >= (Int)right,
               //[(ObjectTypes.Long, ObjectTypes.Float)] = (left, right) => (Long)left >= (Float)right,
               [(ObjectTypes.Long, ObjectTypes.Long)] = (left, right) => (Long)left >= (Long)right,
           };

        private MoreOrEquality() { }

        static MoreOrEquality()
        {
            Instance = new MoreOrEquality();
        }

        public BinaryOperationKind Kind => BinaryOperationKind.MoreOrEquality;

        public static MoreOrEquality Instance { get; }

        public Object Evaluate(Object left, Object right)
        {
            if (!_evaluations.TryGetValue((left.Type, right.Type), out var evaluation))
                throw new System.InvalidOperationException();

            return evaluation.Invoke(left, right);
        }

        public ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public bool IsApplicable(ObjectTypes left, ObjectTypes right)
        {
           return left == ObjectTypes.Int && right == ObjectTypes.Float
                || left == ObjectTypes.Int && right == ObjectTypes.Long
                || left == ObjectTypes.Float && right == ObjectTypes.Int
                || left == ObjectTypes.Long && right == ObjectTypes.Int;
        }
    }
}
