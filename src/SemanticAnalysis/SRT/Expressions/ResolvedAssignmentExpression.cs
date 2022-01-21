using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedAssignmentExpression : ResolvedExpression
    {
        public ResolvedAssignmentExpression(Variable variable, ResolvedExpression expression, TextLocation equalsLocation)
        {
            Variable = variable;
            Expression = expression;
            EqualsLocation = equalsLocation;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.AssignmentExpression;
        public override ObjectTypes Type => Variable?.Type ?? ObjectTypes.Unknown;
        public Variable Variable { get; }
        public ResolvedExpression Expression { get; }
        public TextLocation EqualsLocation { get; }
    }
}