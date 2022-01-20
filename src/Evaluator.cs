using System.Collections.Generic;
using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class Evaluator
    {
        private Diagnostic _diagnostic;
        private Dictionary<Variable, Object> _scope; 

        public CompilationState<Object> Evaluate(SourceCode code, ResolvedNode node, Dictionary<Variable, Object> scope)
        {
            _diagnostic = new Diagnostic(code);
            _scope = scope;

            Object result = null;

            switch (node.Kind)
            {
                case ResolvedNodeKind.DeclareVariableStatement:
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
                case ResolvedNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((ResolvedLiteralExpression)expression);

                case ResolvedNodeKind.ParenthesizedExpression:
                    return EvaluateExpression((expression as ResolvedParenthesizedExpression).Expression);

                case ResolvedNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((ResolvedUnaryExpression)expression);

                case ResolvedNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((ResolvedBinaryExpression)expression);
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

            if (binary.Operation.Kind == BinaryOperationKind.Division
                && result is null)
            {
                _diagnostic.ReportDivisionByZero(binary.OperatorLocation);
            }

            return result;
        }

        #endregion

        #region Statements

        private Object EvaluateStatement(ResolvedStatement statement)
        {
            switch (statement.Kind)
            {
                case ResolvedNodeKind.DeclareVariableStatement:
                    return EvaluateDeclareVariableStatement((ResolvedDeclareVariableStatement)statement);
            }

            throw new System.Exception($"Unknown '{statement.Kind}' is not evaluated");
        }

        private Object EvaluateDeclareVariableStatement(ResolvedDeclareVariableStatement statement)
        {
            var variable = statement.Variable;
            if (variable == null)
                return null;

            var value = statement.InitializedExpression == null 
                ? Object.Create(variable.Type) 
                : EvaluateExpression(statement.InitializedExpression);

            if (variable.Type != value.Type)
            {
                if (ImplicitCast.Instance.IsApplicable(value.Type, variable.Type))
                {
                    value = ImplicitCast.Instance.CastTo(variable.Type, value);
                }
                else
                {
                    _diagnostic.ReportImpossibleImplicitCast(value.Type, variable.Type, statement.EqualsLocation.Value);
                    value = null;
                }
            }

            _scope[variable] = value ?? Object.Create(variable.Type);

            return null;
        }

        #endregion
    }
}
