using System.Collections.Generic;
using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class Evaluator
    {
        private Diagnostic _diagnostic;
        private Dictionary<string, Variable> _scope;

        public CompilationState<Object> Evaluate(SourceCode code, ResolvedNode node, Dictionary<string, Variable> scope)
        {
            _diagnostic = new Diagnostic(code);
            _scope = scope;

            Object result = null;

            switch (node.Kind)
            {
                case ResolvedNodeKind.VariableDeclarationStatement:
                    {
                        result = EvaluateStatement((ResolvedStatement)node);
                        break;
                    }

                default:
                    {
                        result = EvaluateExpression((ResolvedExpression)node);
                        break;
                    }
            }

            return new CompilationState<Object>(result, _diagnostic.Errors);
        }

        #region Expressions

        public Object EvaluateExpression(ResolvedExpression expression)
        {
            switch (expression.Kind)
            {
                case ResolvedNodeKind.LostExpression:
                    return null;

                case ResolvedNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((ResolvedLiteralExpression)expression);

                case ResolvedNodeKind.IdentifierExpression:
                    return EvaluateIdentifierExpression((ResolvedIdentifierExpression)expression);

                case ResolvedNodeKind.ParenthesizedExpression:
                    return EvaluateExpression((expression as ResolvedParenthesizedExpression).Expression);

                case ResolvedNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((ResolvedUnaryExpression)expression);

                case ResolvedNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((ResolvedBinaryExpression)expression);

                case ResolvedNodeKind.AssignmentExpression:
                    return EvaluateAssignmentExpression((ResolvedAssignmentExpression)expression);
            }

            throw new System.Exception($"Unknown '{expression.Kind}' is not evaluated");
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

        private Object EvaluateIdentifierExpression(ResolvedIdentifierExpression expression)
        {
            return expression.Variable?.Value ?? null;
        }

        private Object EvaluateUnaryExpression(ResolvedUnaryExpression unary)
        {
            var operand = EvaluateExpression(unary.Operand);

            return unary.Operation.Evaluate(operand);
        }

        private Object EvaluateBinaryExpression(ResolvedBinaryExpression binary)
        {
            var left = EvaluateExpression(binary.Left);
            var right = EvaluateExpression(binary.Right);

            var result = binary.Operation.Evaluate(left, right);

            if (binary.Operation.Kind == BinaryOperationKind.Division && result is null)
            {
                _diagnostic.ReportDivisionByZero(binary.OperatorLocation);
            }

            return result;
        }

        private Object EvaluateAssignmentExpression(ResolvedAssignmentExpression expression)
        {
            var variable = expression.Variable;
            var value = EvaluateExpression(expression.Expression);

            if (variable == null || value == null)
            {
                _diagnostic.ReportCannotAssignValueError(expression.EqualsLocation);

                return null;
            }
            
            variable.SetValue(ImplicitCast.Instance.CastTo(variable.Type, value));

            return value;
        }

        #endregion

        #region Statements

        private Object EvaluateStatement(ResolvedStatement statement)
        {
            switch (statement.Kind)
            {
                case ResolvedNodeKind.VariableDeclarationStatement:
                    return EvaluateVariableDeclarationStatement((ResolveVariableDeclarationStatement)statement);
            }

            throw new System.Exception($"Unknown '{statement.Kind}' is not evaluated");
        }

        private Object EvaluateVariableDeclarationStatement(ResolveVariableDeclarationStatement statement)
        {
            var variable = statement.Variable;

            if (variable == null)
            {
                if (statement.EqualsLocation != null)
                    _diagnostic.ReportCannotAssignValueError(statement.EqualsLocation.Value);

                return null;
            }

            var value = statement.InitializedExpression == null 
                ? Object.Create(variable.Type) 
                : EvaluateExpression(statement.InitializedExpression);

            if (variable.Type != value.Type)
                value = ImplicitCast.Instance.CastTo(variable.Type, value);

            _scope[variable.Name].SetValue(value);

            return null;
        }

        #endregion
    }
}
