using System;
using System.Collections.Generic;
using Translator.SRT;

namespace Translator
{
    internal sealed class Evaluator
    {
        private readonly List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;

        public int? Evaluate(ResolvedExpression expression)
        {
            _errors.Clear();

            return EvaluateExpression(expression);
        }

        public int? EvaluateExpression(ResolvedExpression expression)
        {
            switch (expression.Kind)
            {
                case ResolvedNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((ResolvedLiteralExpression)expression);

                case ResolvedNodeKind.ParenthesizedExpression:
                    return Evaluate((expression as ResolvedParenthesizedExpression).Expression);

                case ResolvedNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((ResolvedUnaryExpression)expression);

                case ResolvedNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((ResolvedBinaryExpression)expression);
            }

            throw new Exception($"Unknown expression's type '{expression.Kind}'");
        }

        private int? EvaluateLiteralExpression(ResolvedLiteralExpression literal)
        {
            if (literal.ReturnedType == typeof(int) && int.TryParse(literal.Value, out var value))
                return value;

            return null;
        }

        private int? EvaluateUnaryExpression(ResolvedUnaryExpression unary)
        {
            switch (unary.Operation)
            {
                case UnaryOperation.Positive:
                    return Evaluate(unary.Operand);

                case UnaryOperation.Negation:
                    return -Evaluate(unary.Operand);
            }

            throw new Exception($"Unknown unary operation's type '{unary.Operation}'");
        }

        private int? EvaluateBinaryExpression(ResolvedBinaryExpression bin)
        {
            var left = Evaluate(bin.Left);
            var right = Evaluate(bin.Right);

            if (left is null || right is null)
                return null;

            switch (bin.Operation)
            {
                case BinaryOperation.Addition:
                    return left + right;

                case BinaryOperation.Subtraction:
                    return left - right;

                case BinaryOperation.Multiplication:
                    return left * right;

                case BinaryOperation.Division:
                {
                    if (right == 0)
                    {
                        _errors.Add($"ERROR: Division by zero");
                        return null;
                    }

                    return left / right;
                }

                case BinaryOperation.Exponentiation:
                    return (int)Math.Pow((int)left, (int)right);
            }

            throw new Exception($"Unknown binary operation's type '{bin.Operation}'");
        }
    }
}
