using Translator.ObjectModel;

namespace Translator
{
    internal sealed class LogicalNegation : IUnaryOperation
    {
        private LogicalNegation() { }

        static LogicalNegation()
        {
            Instance = new LogicalNegation();
        }

        public UnaryOperationKind Kind => UnaryOperationKind.LogicalNegation;

        public static LogicalNegation Instance { get; }

        public Object Evaluate(Object operand)
        {
            if (operand.Type == ObjectTypes.Bool)
                return new Bool(!(operand as Bool).Value);

            throw new System.InvalidOperationException();
        }

        public ObjectTypes GetResultObjectType(ObjectTypes operand) => operand;

        public bool IsApplicable(ObjectTypes operand) => operand == ObjectTypes.Bool;
    }
}
