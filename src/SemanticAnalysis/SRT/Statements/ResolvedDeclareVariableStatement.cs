using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedDeclareVariableStatement : ResolvedStatement
    {
        public ResolvedDeclareVariableStatement(Variable identifier, ResolvedExpression initializedExpression, TextLocation? equalsLocation)
        {
            Variable = identifier;
            InitializedExpression = initializedExpression;
            EqualsLocation = equalsLocation;
        }

        public override ResolvedNodeKind Kind => ResolvedNodeKind.DeclareVariableStatement;
        public Variable Variable { get; }
        public ResolvedExpression InitializedExpression { get; }
        public TextLocation? EqualsLocation { get; }
    }
}
