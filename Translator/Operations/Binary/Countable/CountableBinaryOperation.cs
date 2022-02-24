using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal abstract class NumberBinaryOperation : BinaryOperation
    {
        private readonly Dictionary<(ObjectTypes, ObjectTypes), ObjectTypes> _resultObjects
            = new Dictionary<(ObjectTypes, ObjectTypes), ObjectTypes>()
            {
                [(ObjectTypes.Int, ObjectTypes.Int)] = ObjectTypes.Int,
                [(ObjectTypes.Int, ObjectTypes.Float)] = ObjectTypes.Float,
                [(ObjectTypes.Int, ObjectTypes.Long)] = ObjectTypes.Long,

                [(ObjectTypes.Float, ObjectTypes.Int)] = ObjectTypes.Float,
                [(ObjectTypes.Float, ObjectTypes.Float)] = ObjectTypes.Float,
                //[(ObjectTypes.Float, ObjectTypes.Long)] = ObjectTypes.Long,

                [(ObjectTypes.Long, ObjectTypes.Int)] = ObjectTypes.Long,
                //[(ObjectTypes.Long, ObjectTypes.Float)] = ObjectTypes.Long,
                [(ObjectTypes.Long, ObjectTypes.Long)] = ObjectTypes.Long,
            };

        public override ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => _resultObjects[(left, right)];

        public override bool IsApplicable(ObjectTypes left, ObjectTypes right) => _resultObjects.ContainsKey((left, right));

        public override BinaryOperationKind Kind { get; }
    }
}
