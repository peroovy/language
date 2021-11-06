namespace Translator.AST
{
    internal abstract class SyntaxNode
    {
        public abstract SyntaxNodeKind Kind { get; }
    }
}
