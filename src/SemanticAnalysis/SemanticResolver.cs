using System;
using System.Collections.Generic;
using Translator.AST;
using Translator.SRT;

namespace Translator
{
    internal sealed class SemanticResolver
    {
        private readonly SyntaxExpression _expression;

        private readonly Diagnostic _diagnostic;

        public SemanticResolver(SourceCode code, SyntaxExpression expression)
        {
            _expression = expression;
            _diagnostic = new Diagnostic(code);
        }

        public IEnumerable<Error> Errors => _diagnostic.Errors;

        public ResolvedExpression Resolve()
        {
            return ResolveExpression(_expression);
        }

        private ResolvedExpression ResolveExpression(SyntaxExpression expression)
        {
            switch (expression.Kind)
            {
                case SyntaxNodeKind.LiteralExpression:
                    return ResolveLiteralExpression((SyntaxLiteralExpression)expression);

                case SyntaxNodeKind.ParenthesizedExpression:
                    return ResolveParenthesizedExpression((SyntaxParenthesizedExpression)expression);

                case SyntaxNodeKind.UnaryExpression:
                    return ResolveUnaryExpression((SyntaxUnaryExpression)expression);

                case SyntaxNodeKind.BinaryExpression:
                    return ResolveBinaryExpression((SyntaxBinaryExpression)expression);
            }

            throw new Exception($"'{expression.Kind}' is not resolved");
        }

        private ResolvedLiteralExpression ResolveLiteralExpression(SyntaxLiteralExpression literal)
        {
            switch (literal.Token.Type)
            {
                case TokenType.Number:
                    return new ResolvedLiteralExpression(literal.Token.Value, typeof(int));

                case TokenType.TrueKeyword:
                case TokenType.FalseKeyword:
                    return new ResolvedLiteralExpression(literal.Token.Value, typeof(bool));
            }

            throw new Exception($"{literal.Kind} '{literal.Token.Value}' is not resolved");
        }

        private ResolvedParenthesizedExpression ResolveParenthesizedExpression(SyntaxParenthesizedExpression parentheses)
        {
            var resolvedExpression = ResolveExpression(parentheses.Expression);

            return new ResolvedParenthesizedExpression(resolvedExpression);
        }

        private ResolvedExpression ResolveUnaryExpression(SyntaxUnaryExpression unary)
        {
            var operand = ResolveExpression(unary.Operand);
            var operation = UnaryOperation.Resolve(unary.OperatorToken.Type, operand);

            if (operation is null)
                throw new Exception($"The unary operator '{unary.OperatorToken.Value}' is not resolved");

            if (operation.ReturnedType != null)
                return new ResolvedUnaryExpression(operation, operand);

            _diagnostic.ReportUndefinedUnaryOperationForType(operation, unary.OperatorToken.Location);

            return operand;
        }

        private ResolvedExpression ResolveBinaryExpression(SyntaxBinaryExpression bin)
        {
            var left = ResolveExpression(bin.Left);
            var right = ResolveExpression(bin.Right);
            var operation = BinaryOperation.Resove(bin.OperatorToken.Type, left, right);

            if (operation is null)
                throw new Exception($"The binary operator '{bin.OperatorToken.Value}' is not resolved");

            if (operation.ReturnedType != null)
                return new ResolvedBinaryExpression(left, operation, right);

            _diagnostic.ReportUndefinedBinaryOperationForTypes(operation, bin.OperatorToken.Location);

            return left;
        }
    }
}
