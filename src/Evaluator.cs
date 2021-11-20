using System;
using System.Collections.Generic;
using Translator.SRT;

namespace Translator
{
    internal sealed class Evaluator
    {
        private readonly ResolvedExpression _expression;

        private readonly Diagnostic _diagnostic;

        public Evaluator(SourceCode code, ResolvedExpression expression)
        {
            _expression = expression;
            _diagnostic = new Diagnostic(code);
        }

        public IEnumerable<Error> Errors => _diagnostic.Errors;

        public object Evaluate()
        {
            return EvaluateExpression(_expression);
        }

        public object EvaluateExpression(ResolvedExpression expression)
        {
            switch (expression.Kind)
            {
                case ResolvedNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((ResolvedLiteralExpression)expression);

                case ResolvedNodeKind.ParenthesizedExpression:
                    return EvaluateExpression((expression as ResolvedParenthesizedExpression).Expression);

                case ResolvedNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((ResolvedUnaryExpression)expression);

                case ResolvedNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((ResolvedBinaryExpression)expression);
            }

            throw new Exception($"Unknown expression's type '{expression.Kind}'");
        }

        private object EvaluateLiteralExpression(ResolvedLiteralExpression literal)
        {
            if (literal.ReturnedType == typeof(int))
                return int.TryParse(literal.Value, out var value) ? value : 0;

            if (literal.ReturnedType == typeof(bool))
                return bool.TryParse(literal.Value, out var value) ? value : false;

            throw new Exception($"Unknown literal {literal.Kind}'");
        }

        private object EvaluateUnaryExpression(ResolvedUnaryExpression unary)
        {
            switch (unary.Operation.Kind)
            {
                case UnaryOperationKind.Positive:
                    return EvaluateExpression(unary.Operand);

                case UnaryOperationKind.Negation:
                    return -(int)EvaluateExpression(unary.Operand);

                case UnaryOperationKind.LogicalNegation:
                    return !(bool)EvaluateExpression(unary.Operand);
            }

            throw new Exception($"Unknown unary operation's type '{unary.Operation}'");
        }

        private object EvaluateBinaryExpression(ResolvedBinaryExpression bin)
        {
            var left = EvaluateExpression(bin.Left);
            var right = EvaluateExpression(bin.Right);

            if (left is null || right is null)
                return null;

            switch (bin.Operation.Kind)
            {
                case BinaryOperationKind.Addition:
                    return (int)left + (int)right;

                case BinaryOperationKind.Subtraction:
                    return (int)left - (int)right;

                case BinaryOperationKind.Multiplication:
                    return (int)left * (int)right;

                case BinaryOperationKind.Division:
                {
                    if ((int)right == 0)
                    {
                        // _diagnostic.ReportDivisionByZero();

                        return null;
                    }

                    return (int)left / (int)right;
                }

                case BinaryOperationKind.Exponentiation:
                    return (int)Math.Pow((int)left, (int)right);

                case BinaryOperationKind.LogicalAnd:
                    return (bool)left && (bool)right;

                case BinaryOperationKind.LogicalOr:
                    return (bool)left || (bool)right;
            }

            throw new Exception($"Unknown binary operation's type '{bin.Operation}'");
        }
    }
}
