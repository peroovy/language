namespace Translator.AST
{
    internal sealed class LiteralExpression : Expression
    {
        public LiteralExpression(Token literal)
        {
            Literal = literal;
        }

        public Token Literal { get; }
    }
}
