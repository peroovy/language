using Translator.AST;

namespace Translator
{
    internal sealed class SyntaxExplicitCastExpression : SyntaxExpression
    {
        public SyntaxExplicitCastExpression(
            Token openParenthesis, Token keyword, Token closeParenthesis, SyntaxExpression expression)
        {
            OpenParenthesis = openParenthesis;
            Keyword = keyword;
            CloseParenthesis = closeParenthesis;
            Expression = expression;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.ExplicitCastExpression;
        public Token OpenParenthesis { get; }
        public Token Keyword { get; }
        public Token CloseParenthesis { get; }
        public SyntaxExpression Expression { get; }
    }
}
