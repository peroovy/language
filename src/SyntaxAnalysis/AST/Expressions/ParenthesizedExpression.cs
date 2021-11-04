﻿namespace Translator.AST
{
    internal sealed class ParenthesizedExpression : Expression
    {
        public ParenthesizedExpression(Token openParenthesis, Expression expression, Token closeParenthesis)
        {
            OpenParenthesis = openParenthesis;
            Expression = expression;
            CloseParenthesis = closeParenthesis;
        }

        public Token OpenParenthesis { get; }
        public Expression Expression { get; }
        public Token CloseParenthesis { get; }
    }
}