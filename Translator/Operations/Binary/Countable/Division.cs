using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Division : NumberBinaryOperation
    {
        private Division() { }

        static Division()
        {
            Instance = new Division();
        }

        public override BinaryOperationKind Kind => BinaryOperationKind.Division;

        public static Division Instance { get; }

        public override Object Evaluate(Int left, Int right) => right.Value == 0 ? null : new Int(left.Value / right.Value);

        public override Object Evaluate(Int left, Float right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

        public override Object Evaluate(Int left, Long right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Float left, Int right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

        public override Object Evaluate(Float left, Float right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

        public override Object Evaluate(Float left, Long right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Int right)
        {
            var longRight = (Long)ImplicitCasting.Instance.Apply(right, ObjectTypes.Long);

            return Evaluate(left, longRight);
        }

        public override Object Evaluate(Long left, Float right)
        {
            throw new System.NotImplementedException();
        }

        public override Object Evaluate(Long left, Long right)
        {
            throw new System.NotImplementedException();
        }
    }
}
