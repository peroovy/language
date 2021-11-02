using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.AST
{
    abstract class SyntaxNode
    {
    }

    abstract class Expression : SyntaxNode
    {
    }

    sealed class BinaryExpression : Expression
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

    sealed class LiteralExpression : Expression
    {
        public LiteralExpression(Token literal)
        {
            Literal = literal;
        }

        public Token Literal { get; }
    }
}
