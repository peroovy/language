﻿using Translator.ObjectModel;

namespace Translator
{
    internal sealed class MoreOrEquality : IBinaryOperation
    {
        private MoreOrEquality() { }

        static MoreOrEquality()
        {
            Instance = new MoreOrEquality();
        }

        public BinaryOperationKind Kind => BinaryOperationKind.MoreOrEquality;

        public static MoreOrEquality Instance { get; }

        public Object Evaluate(Object left, Object right)
        {
            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Int)
                return (Int)left >= (Int)right;

            if (left.Type == ObjectTypes.Int && right.Type == ObjectTypes.Float)
                return (Int)left >= (Float)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Int)
                return (Float)left >= (Int)right;

            if (left.Type == ObjectTypes.Float && right.Type == ObjectTypes.Float)
                return (Float)left >= (Float)right;

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public bool IsApplicable(ObjectTypes left, ObjectTypes right)
        {
            return left == ObjectTypes.Int && right == ObjectTypes.Int
                || left == ObjectTypes.Float && right == ObjectTypes.Float
                || left == ObjectTypes.Int && right == ObjectTypes.Float
                || left == ObjectTypes.Float && right == ObjectTypes.Int;
        }
    }
}
