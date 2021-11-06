﻿using System;
using System.Collections.Generic;
using Translator.AST;
using Translator.SRT;

namespace Translator
{
    internal sealed class SemanticResolver
    {
        private readonly Expression _expression;

        private readonly Diagnostic _diagnostic;

        public SemanticResolver(SourceCode code, Expression expression)
        {
            _expression = expression;
            _diagnostic = new Diagnostic(code);
        }

        public IEnumerable<Error> Errors => _diagnostic.Errors;

        public ResolvedExpression Resolve()
        {
            return ResolveExpression(_expression);
        }

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
            var resolvedExpression = ResolveExpression(parentheses.Expression);

            return new ResolvedParenthesizedExpression(resolvedExpression);
        }

        private ResolvedExpression ResolveUnaryExpression(UnaryExpression unary)
        {
            var resolvedOperand = ResolveExpression(unary.Operand);
            var operation = unary.OperatorToken.Type.ToUnaryOperation();

            if (operation is null)
                throw new Exception($"The unary operator '{unary.OperatorToken.Value}' is not defined");

            if (resolvedOperand.ReturnedType != typeof(int))
            {
                _diagnostic.ReportUndefinedUnaryOperationFor(resolvedOperand.ReturnedType, operation.Value, unary.OperatorToken.Location);

                return resolvedOperand;
            }

            return new ResolvedUnaryExpression(operation.Value, resolvedOperand);
        }

        private ResolvedExpression ResolveBinaryExpression(BinaryExpression bin)
        {
            var left = ResolveExpression(bin.Left);
            var right = ResolveExpression(bin.Right);
            var operation = bin.OperatorToken.Type.ToBinaryOperation();

            if (operation is null)
                throw new Exception($"The binary operator '{bin.OperatorToken.Value}' is not defined");

            if (left.ReturnedType != typeof(int) || right.ReturnedType != typeof(int))
            {
                _diagnostic.ReportUndefinedBinaryOperationFor(left.ReturnedType, right.ReturnedType, operation.Value, bin.OperatorToken.Location);
                
                return left;
            }

            return new ResolvedBinaryExpression(left, operation.Value, right);
        }
    }
}