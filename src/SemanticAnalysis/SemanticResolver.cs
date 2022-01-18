using System;
using Translator.AST;
using Translator.SRT;

namespace Translator
{
    internal sealed class SemanticResolver
    {
        private Diagnostic _diagnostic;

        public CompilationState<ResolvedExpression> Resolve(SourceCode code, SyntaxExpression expression)
        {
            _diagnostic = new Diagnostic(code);

            var expr = ResolveExpression(expression);

            return new CompilationState<ResolvedExpression>(expr, _diagnostic.Errors);
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
            return new ResolvedLiteralExpression(literal.Token.Value, literal.ObjectType);
        }

        private ResolvedParenthesizedExpression ResolveParenthesizedExpression(SyntaxParenthesizedExpression parentheses)
        {
            var resolvedExpression = ResolveExpression(parentheses.Expression);

            return new ResolvedParenthesizedExpression(resolvedExpression);
        }

        private ResolvedExpression ResolveUnaryExpression(SyntaxUnaryExpression unary)
        {
            var operand = ResolveExpression(unary.Operand);
            var operation = unary.OperatorToken.Type.ToUnaryOperation();

            if (operation is null)
                throw new Exception($"The unary operator '{unary.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(operand.Type))
                return new ResolvedUnaryExpression(operation, operand);

            _diagnostic.ReportUndefinedUnaryOperationForType(operation.Kind, operand.Type, unary.OperatorToken.Location);

            return operand;
        }

        private ResolvedExpression ResolveBinaryExpression(SyntaxBinaryExpression bin)
        {
            var left = ResolveExpression(bin.Left);
            var right = ResolveExpression(bin.Right);
            var operation = bin.OperatorToken.Type.ToBinaryOperation();

            if (operation is null)
                throw new Exception($"The binary operator '{bin.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(left.Type, right.Type))
                return new ResolvedBinaryExpression(left, operation, right, bin.OperatorToken.Location);

            _diagnostic.ReportUndefinedBinaryOperationForTypes(left.Type, operation.Kind, right.Type, bin.OperatorToken.Location);

            return left;
        }
    }
}