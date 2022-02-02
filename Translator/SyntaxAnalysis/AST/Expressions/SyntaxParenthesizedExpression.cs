namespace Translator.AST
{
    internal sealed class SyntaxParenthesizedExpression : SyntaxExpression
    {
        public SyntaxParenthesizedExpression(Token openParenthesis, SyntaxExpression expression, Token closeParenthesis)
        {
            OpenParenthesis = openParenthesis;
            Expression = expression;
            CloseParenthesis = closeParenthesis;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.ParenthesizedExpression;
        public Token OpenParenthesis { get; }
        public SyntaxExpression Expression { get; }
        public Token CloseParenthesis { get; }
    }
}
