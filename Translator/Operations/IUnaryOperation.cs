using Translator.ObjectModel;

namespace Translator
{
    internal interface IUnaryOperation
    {
        UnaryOperationKind Kind { get; }

        bool IsApplicable(ObjectTypes operand);
        ObjectTypes GetResultObjectType(ObjectTypes operand);
        Object Evaluate(Object operand);
    }
}
