using System;
using Translator.AST;

namespace Translator
{
    internal sealed class Evaluator
    {
        public int Evaluate(Expression expression)
        {
            if (expression is LiteralExpression number)
                return int.TryParse(number.Literal.Value, out var result) ? result : 0;

            if (expression is ParenthesizedExpression p)
                return Evaluate(p.Expression);

            if (expression is UnaryExpression un)
            {
                switch (un.OperatorToken.Type)
                {
                    case TokenType.Plus:
                        return Evaluate(un.Operand);

                    case TokenType.Minus:
                        return -Evaluate(un.Operand);
                }
            }

            if (expression is BinaryExpression bin)
            {
                var left = Evaluate(bin.Left);
                var right = Evaluate(bin.Right);

                switch (bin.OperatorToken.Type)
                {
                    case TokenType.Plus:
                        return left + right;

                    case TokenType.Minus:
                        return left - right;

                    case TokenType.Star:
                        return left * right;

                    case TokenType.Slash:
                        return left / right;

                    case TokenType.StarStar:
                        return (int)Math.Pow(left, right);
                }

                return left;
            }

            return 0;
        }
    }
}
