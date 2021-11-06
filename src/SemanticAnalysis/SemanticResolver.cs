using System;
using System.Collections.Generic;
using Translator.AST;
using Translator.SRT;

namespace Translator
{
    internal sealed class SemanticResolver
    {
        private readonly List<string> _errors = new List<string>();

        public ResolvedExpression Resolve(Expression expression)
        {
            _errors.Clear();

            return ResolveExpression(expression);
        }

        public IEnumerable<string> Errors => _errors;

        private ResolvedExpression ResolveExpression(Expression expression)
        {
            switch (expression.Kind)
            {
                case SyntaxNodeKind.LiteralExpression:
                    return ResolveLiteralExpression((LiteralExpression)expression);

                case SyntaxNodeKind.ParenthesizedExpression:
                    return ResolveParenthesizedExpression((ParenthesizedExpression)expression);

                case SyntaxNodeKind.UnaryExpression:
                    return ResolveUnaryExpression((UnaryExpression)expression);

                case SyntaxNodeKind.BinaryExpression:
                    return ResolveBinaryExpression((BinaryExpression)expression);
            }

            throw new Exception($"'{expression.Kind}' is not resolved");
        }

        private ResolvedLiteralExpression ResolveLiteralExpression(LiteralExpression literal)
        {
            switch (literal.Token.Type)
            {
                case TokenType.Number:
                    return new ResolvedLiteralExpression(literal.Token.Value, typeof(int));
            }

            throw new Exception($"{literal.Kind} '{literal.Token.Value}' is not resolved");
        }

        private ResolvedParenthesizedExpression ResolveParenthesizedExpression(ParenthesizedExpression parentheses)
        {
            var resolvedExpression = Resolve(parentheses.Expression);

            return new ResolvedParenthesizedExpression(resolvedExpression);
        }

        private ResolvedExpression ResolveUnaryExpression(UnaryExpression unary)
        {
            var resolvedOperand = Resolve(unary.Operand);
            var operation = unary.OperatorToken.Type.ToUnaryOperation();

            if (operation is null)
                throw new Exception($"The unary operator '{unary.OperatorToken.Value}' is not defined");

            if (resolvedOperand.ReturnedType != typeof(int))
            {
                _errors.Add($"ERROR: The unary operation '{operation}' is not defined for type '{resolvedOperand.ReturnedType}'");

                return resolvedOperand;
            }

            return new ResolvedUnaryExpression(operation.Value, resolvedOperand);
        }

        private ResolvedExpression ResolveBinaryExpression(BinaryExpression bin)
        {
            var resolvedLeft = Resolve(bin.Left);
            var resolvedRight = Resolve(bin.Right);
            var operation = bin.OperatorToken.Type.ToBinaryOperation();

            if (operation is null)
                throw new Exception($"The binary operator '{bin.OperatorToken.Value}' is not defined");

            if (resolvedLeft.ReturnedType != typeof(int) || resolvedRight.ReturnedType != typeof(int))
            {
                _errors.Add($"ERROR: The binary operator '{bin.OperatorToken.Value}' is not defined for types '{resolvedLeft.ReturnedType}' and '{resolvedRight.ReturnedType}'");

                return resolvedLeft;
            }

            return new ResolvedBinaryExpression(resolvedLeft, operation.Value, resolvedRight);
        }
    }
}
