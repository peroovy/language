using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.AST;

namespace Translator
{
    sealed class Evaluator
    {
        public int Evaluate(Expression expression)
        {
            if (expression is LiteralExpression number)
                return int.TryParse(number.Literal.Value, out var result) ? result : 0;

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
                }

                return left;
            }

            return 0;
        }
    }
}
