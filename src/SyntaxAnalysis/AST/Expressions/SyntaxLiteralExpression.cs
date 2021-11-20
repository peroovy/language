namespace Translator.AST
{
    internal sealed class SyntaxLiteralExpression : SyntaxExpression
    {
        public SyntaxLiteralExpression(Token token)
        {
            Token = token;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.LiteralExpression;
        public Token Token { get; }
    }
}
