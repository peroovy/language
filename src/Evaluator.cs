using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class Evaluator
    {
        private Diagnostic _diagnostic;

        public CompilationState<Object> Evaluate(SourceCode code, ResolvedExpression expression)
        {
            _diagnostic = new Diagnostic(code);

            var result = EvaluateExpression(expression);

            return new CompilationState<Object>(result, _diagnostic.Errors);
        }

        public Object EvaluateExpression(ResolvedExpression expression)
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

            throw new System.Exception($"Unknown expression's type '{expression.Kind}'");
        }

        private Object EvaluateLiteralExpression(ResolvedLiteralExpression literal)
        {
            switch (literal.Type)
            {
                case ObjectTypes.Int:
                    return Int.Create(literal.Value);

                case ObjectTypes.Float:
                    return Float.Create(literal.Value);

                case ObjectTypes.Bool:
                    return Bool.Create(literal.Value);

                case ObjectTypes.Null:
                    return new Null();

                case ObjectTypes.Unknown:
                    return null;
            }

            throw new System.Exception($"Unknown evaluation of {literal.Type} literal");
        }

        private Object EvaluateUnaryExpression(ResolvedUnaryExpression unary)
        {
            var operand = EvaluateExpression(unary.Operand);

            return unary.Operation.Evaluate(operand);
        }

        private Object EvaluateBinaryExpression(ResolvedBinaryExpression bin)
        {
            var left = EvaluateExpression(bin.Left);
            var right = EvaluateExpression(bin.Right);

            var result = bin.Operation.Evaluate(left, right);

            if (bin.Operation.Kind == BinaryOperationKind.Division
                && result is null)
            {
                _diagnostic.ReportDivisionByZero(bin.OperatorLocation);
            }

            return result;
        }
    }
}
