using Translator.AST;

namespace Translator
{
    internal sealed class SyntaxIdentifierExpression : SyntaxExpression
    {
        public SyntaxIdentifierExpression(Token name)
        {
            Name = name;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.IdentifierExpression;
        public Token Name { get; }
    }
}
