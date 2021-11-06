namespace Translator.AST
{
    internal sealed class LiteralExpression : Expression
    {
        public LiteralExpression(Token literal)
        {
            Token = literal;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.LiteralExpression;
        public Token Token { get; }
    }
}
