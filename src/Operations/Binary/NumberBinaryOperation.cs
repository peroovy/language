using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal abstract class NumberBinaryOperation : IBinaryOperation
    {
        private readonly Dictionary<(ObjectTypes, ObjectTypes), ObjectTypes> _resultObjects
            = new Dictionary<(ObjectTypes, ObjectTypes), ObjectTypes>()
        {
            [(ObjectTypes.Int, ObjectTypes.Int)] = ObjectTypes.Int,
            [(ObjectTypes.Int, ObjectTypes.Float)] = ObjectTypes.Float,
            [(ObjectTypes.Float, ObjectTypes.Int)] = ObjectTypes.Float,
            [(ObjectTypes.Float, ObjectTypes.Float)] = ObjectTypes.Float
        };

        public ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => _resultObjects[(left, right)];

        public bool IsApplicable(ObjectTypes left, ObjectTypes right) => _resultObjects.ContainsKey((left, right));

        public abstract BinaryOperationKind Kind { get; }

        public abstract Object Evaluate(Object left, Object right);
    }
}
