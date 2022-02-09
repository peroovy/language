using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal abstract class BinaryOperation
    {
        private readonly Dictionary<(ObjectTypes, ObjectTypes), System.Func<Object, Object, Object>> _evaluations;

        public BinaryOperation()
        {
            _evaluations = new Dictionary<(ObjectTypes, ObjectTypes), System.Func<Object, Object, Object>>()
            {
                [(ObjectTypes.Int, ObjectTypes.Int)] = (left, right) => Evaluate((Int)left, (Int)right),
                [(ObjectTypes.Int, ObjectTypes.Float)] = (left, right) => Evaluate((Int)left, (Float)right),
                [(ObjectTypes.Int, ObjectTypes.Bool)] = (left, right) => Evaluate((Int)left, (Bool)right),
                [(ObjectTypes.Int, ObjectTypes.Long)] = (left, right) => Evaluate((Int)left, (Long)right),

                [(ObjectTypes.Float, ObjectTypes.Int)] = (left, right) => Evaluate((Float)left, (Int)right),
                [(ObjectTypes.Float, ObjectTypes.Float)] = (left, right) => Evaluate((Float)left, (Float)right),
                [(ObjectTypes.Float, ObjectTypes.Bool)] = (left, right) => Evaluate((Float)left, (Bool)right),
                [(ObjectTypes.Float, ObjectTypes.Long)] = (left, right) => Evaluate((Float)left, (Long)right),

                [(ObjectTypes.Bool, ObjectTypes.Int)] = (left, right) => Evaluate((Bool)left, (Int)right),
                [(ObjectTypes.Bool, ObjectTypes.Float)] = (left, right) => Evaluate((Bool)left, (Float)right),
                [(ObjectTypes.Bool, ObjectTypes.Bool)] = (left, right) => Evaluate((Bool)left, (Bool)right),
                [(ObjectTypes.Bool, ObjectTypes.Long)] = (left, right) => Evaluate((Bool)left, (Long)right),

                [(ObjectTypes.Long, ObjectTypes.Int)] = (left, right) => Evaluate((Long)left, (Int)right),
                [(ObjectTypes.Long, ObjectTypes.Float)] = (left, right) => Evaluate((Long)left, (Float)right),
                [(ObjectTypes.Long, ObjectTypes.Bool)] = (left, right) => Evaluate((Long)left, (Bool)right),
                [(ObjectTypes.Long, ObjectTypes.Long)] = (left, right) => Evaluate((Long)left, (Long)right),
            };
        }

        public abstract BinaryOperationKind Kind { get; }

        public abstract bool IsApplicable(ObjectTypes left, ObjectTypes right);

        public abstract ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right);

        public virtual Object Evaluate(Object left, Object right)
        {
            if (!_evaluations.TryGetValue((left.Type, right.Type), out var evaluation))
                throw new System.InvalidOperationException();

            return evaluation.Invoke(left, right);
        }

        public virtual Object Evaluate(Int left, Int right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Int left, Float right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Int left, Bool right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Int left, Long right) => throw new System.NotImplementedException();

        public virtual Object Evaluate(Float left, Int right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Float left, Float right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Float left, Bool right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Float left, Long right) => throw new System.NotImplementedException();

        public virtual Object Evaluate(Bool left, Int right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Bool left, Float right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Bool left, Bool right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Bool left, Long right) => throw new System.NotImplementedException();

        public virtual Object Evaluate(Long left, Int right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Long left, Float right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Long left, Bool right) => throw new System.NotImplementedException();
        public virtual Object Evaluate(Long left, Long right) => throw new System.NotImplementedException();
    }
}
