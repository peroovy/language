using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedVariableDeclarationStatement : ResolvedStatement
    {
        public ResolvedVariableDeclarationStatement(Variable variable, ResolvedExpression initializedExpression, TextLocation? equalsLocation)
        {
            Variable = variable;
            InitializedExpression = initializedExpression;
            EqualsLocation = equalsLocation;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.VariableDeclarationStatement;
        public Variable Variable { get; }
        public ResolvedExpression InitializedExpression { get; }
        public TextLocation? EqualsLocation { get; }
    }
}
