using Translator.ObjectModel;

namespace Translator
{
    internal abstract class ComparableBinaryOperation : BinaryOperation
    {
        public override ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right) => ObjectTypes.Bool;

        public override bool IsApplicable(ObjectTypes left, ObjectTypes right)
        {
            return left == ObjectTypes.Int && right == ObjectTypes.Int
                || left == ObjectTypes.Int && right == ObjectTypes.Float
                || left == ObjectTypes.Int && right == ObjectTypes.Long
                || left == ObjectTypes.Float && right == ObjectTypes.Int
                || left == ObjectTypes.Float && right == ObjectTypes.Float
                || left == ObjectTypes.Float && right == ObjectTypes.Long
                || left == ObjectTypes.Long && right == ObjectTypes.Int
                || left == ObjectTypes.Long && right == ObjectTypes.Float
                || left == ObjectTypes.Long && right == ObjectTypes.Long;
        }
    }
}
