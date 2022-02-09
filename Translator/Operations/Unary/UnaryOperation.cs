using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal abstract class UnaryOperation
    {
        private readonly Dictionary<ObjectTypes, System.Func<Object, Object>> _evaluations;

        public UnaryOperation()
        {
            _evaluations = new Dictionary<ObjectTypes, System.Func<Object, Object>>()
            {
                [ObjectTypes.Int] = (operand) => Evaluate((Int)operand),
                [ObjectTypes.Float] = (operand) => Evaluate((Float)operand),
                [ObjectTypes.Bool] = (operand) => Evaluate((Bool)operand),
                [ObjectTypes.Long] = (operand) => Evaluate((Long)operand),
            };
        }

        public abstract UnaryOperationKind Kind { get; }

        public abstract bool IsApplicable(ObjectTypes operand);

        public virtual ObjectTypes GetResultObjectType(ObjectTypes operand) => operand;

        public virtual Object Evaluate(Object operand)
        {
            if (!_evaluations.TryGetValue(operand.Type, out var evaluation))
                throw new System.InvalidOperationException();

            return evaluation.Invoke(operand);
        }

        public virtual Object Evaluate(Int operand) => throw new System.NotImplementedException();

        public virtual Object Evaluate(Float operand) => throw new System.NotImplementedException();

        public virtual Object Evaluate(Bool operand) => throw new System.NotImplementedException();

        public virtual Object Evaluate(Long operand) => throw new System.NotImplementedException();
    }
}
