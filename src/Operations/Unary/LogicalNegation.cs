using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalNegation : IUnaryOperation
    {
        public UnaryOperationKind Kind => UnaryOperationKind.LogicalNegation;

        public Object Evaluate(Object operand)
        {
            if (operand.Kind == ObjectTypes.Bool)
                return new Bool(!(operand as Bool).Value);

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetResultObjectType(ObjectTypes operand) => operand;

        public bool IsApplicable(ObjectTypes operand) => operand == ObjectTypes.Bool;
    }
}
