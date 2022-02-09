using Translator.ObjectModel;

namespace Translator
{
    internal abstract class LogicalBinaryOperation : BinaryOperation
    {
        public override ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public override bool IsApplicable(ObjectTypes left, ObjectTypes right) =>
            left == ObjectTypes.Bool && right == ObjectTypes.Bool;
    }
}
