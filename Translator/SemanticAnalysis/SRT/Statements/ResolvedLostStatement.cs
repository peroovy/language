using Translator.SRT;

namespace Translator
{
    internal sealed class ResolvedLostStatement : ResolvedStatement
    {
        public override ResolvedNodeKind Kind => ResolvedNodeKind.LostStatement;
    }
}
