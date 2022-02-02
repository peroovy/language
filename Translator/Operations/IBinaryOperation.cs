using Translator.ObjectModel;

namespace Translator
{
    internal interface IBinaryOperation
    {
        BinaryOperationKind Kind { get; }

        bool IsApplicable(ObjectTypes left, ObjectTypes right);
        ObjectTypes GetObjectType(ObjectTypes left, ObjectTypes right);

        Object Evaluate(Object left, Object right);
    }
}
