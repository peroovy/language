using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedIdentifierExpression : ResolvedExpression
    {
        public ResolvedIdentifierExpression(Variable variable)
        {
            Variable = variable;
        }
     
        public override ResolvedNodeKind Kind => ResolvedNodeKind.IdentifierExpression;
        public override ObjectTypes Type => Variable?.Type ?? ObjectTypes.Unknown;
        public Variable Variable { get; }
    }
}