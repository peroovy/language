namespace Translator.AST
{
    internal sealed class LiteralExpression : Expression
    {
        public LiteralExpression(Token token)
        {
            Token = token;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.LiteralExpression;
        public Token Token { get; }
    }
}
