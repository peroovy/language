using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedAssignmentExpression : ResolvedExpression
    {
        public ResolvedAssignmentExpression(
            Variable variable, 
            ResolvedExpression expression, 
            BinaryOperation operation, 
            TextLocation equalsLocation)
        {
            Variable = variable;
            Expression = expression;
            Operation = operation;
            OperatorLocation = equalsLocation;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.AssignmentExpression;
        public override ObjectTypes Type => Variable?.Type ?? ObjectTypes.Unknown;
        public Variable Variable { get; }
        public ResolvedExpression Expression { get; }
        public BinaryOperation Operation { get; }
        public TextLocation OperatorLocation { get; }
    }
}