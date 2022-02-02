using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedLostExpression : ResolvedExpression
    {
        public override ResolvedNodeKind Kind => ResolvedNodeKind.LostExpression;
        public override ObjectTypes Type => ObjectTypes.Unknown;
    }
}