﻿namespace Translator.AST
{
    internal sealed class BinaryExpression : Expression
    {
        public BinaryExpression(Expression left, Token operatorToken, Expression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public Expression Left { get; }
        public Token OperatorToken { get; }
        public Expression Right { get; }
    }
}