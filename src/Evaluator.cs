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

            switch (node.Kind)
            {
                case ResolvedNodeKind.VariableDeclarationStatement:
                case ResolvedNodeKind.LostStatement:
                {
                    var statement = EvaluateStatement((ResolvedStatement)node);

                    return new CompilationState<Object>(statement, _diagnostic.Errors);
                }
            }

            var value = EvaluateExpression((ResolvedExpression)node);

            return new CompilationState<Object>(value, _diagnostic.Errors);
        }

        #region Expressions

        public Object EvaluateExpression(ResolvedExpression expression)
        {
            switch (expression.Kind)
            {
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

                case ResolvedNodeKind.LostExpression:
                    return null;
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
            return expression.Variable.Value;
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

            var value = binary.Operation.Evaluate(left, right);

            if (value == null && binary.Operation.Kind == BinaryOperationKind.Division)
                _diagnostic.ReportDivisionByZero(binary.OperatorLocation);

            return value;
        }

        private Object EvaluateAssignmentExpression(ResolvedAssignmentExpression expression)
        {
            var variable = expression.Variable;
            var value = EvaluateExpression(expression.Expression);
            var operation = expression.Operation;

            if (operation != null)
                value = operation.Evaluate(variable.Value, value);

            if (value == null && operation.Kind == BinaryOperationKind.Division)
            {
                _diagnostic.ReportDivisionByZero(expression.OperatorLocation);

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
                    return EvaluateVariableDeclarationStatement((ResolvedVariableDeclarationStatement)statement);

                case ResolvedNodeKind.LostStatement:
                    return null;
            }

            throw new System.Exception($"Unknown '{statement.Kind}' is not evaluated");
        }

        private Object EvaluateVariableDeclarationStatement(ResolvedVariableDeclarationStatement statement)
        {
            var variable = statement.Variable;
            var value = statement.InitializedExpression == null 
                ? Object.Create(variable.Type) 
                : EvaluateExpression(statement.InitializedExpression);

            _scope[variable.Name].SetValue(ImplicitCast.Instance.CastTo(variable.Type, value));

            return null;
        }

        #endregion
    }
}
