using System.Collections.Generic;
using Translator.AST;
using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class SemanticResolver
    {
        private Diagnostic _diagnostic;
        private Dictionary<Variable, Object> _scope;

        public CompilationState<ResolvedNode> Resolve(SourceCode code, SyntaxNode node, Dictionary<Variable, Object> scope)
        {
            _diagnostic = new Diagnostic(code);
            _scope = scope;

            ResolvedNode resolvedNode = null;

            switch (node.Kind)
            {
                case SyntaxNodeKind.DeclareVariableStatement:
                {
                    resolvedNode = ResolveStatement((SyntaxStatement)node);
                    break;
                }

                default:
                {
                    resolvedNode = ResolveExpression((SyntaxExpression)node);
                    break;
                }
            }

            return new CompilationState<ResolvedNode>(resolvedNode, _diagnostic.Errors);
        }

        #region Expressions

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

            throw new System.Exception($"Expression '{expression.Kind}' is not resolved");
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
                throw new System.Exception($"The unary operator '{unary.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(operand.Type))
                return new ResolvedUnaryExpression(operation, operand);

            _diagnostic.ReportUndefinedUnaryOperationForType(operation.Kind, operand.Type, unary.OperatorToken.Location);

            return operand;
        }

        private ResolvedExpression ResolveBinaryExpression(SyntaxBinaryExpression binary)
        {
            var left = ResolveExpression(binary.Left);
            var right = ResolveExpression(binary.Right);
            var operation = binary.OperatorToken.Type.ToBinaryOperation();

            if (operation is null)
                throw new System.Exception($"The binary operator '{binary.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(left.Type, right.Type))
                return new ResolvedBinaryExpression(left, operation, right, binary.OperatorToken.Location);

            _diagnostic.ReportUndefinedBinaryOperationForTypes(left.Type, operation.Kind, right.Type, binary.OperatorToken.Location);

            return left;
        }

        #endregion

        #region Statements

        private ResolvedStatement ResolveStatement(SyntaxStatement statement)
        {
            switch (statement.Kind)
            {
                case SyntaxNodeKind.DeclareVariableStatement:
                    return ResolveDeclareVariableStatement((SyntaxDeclareVariableStatement)statement);
            }

            throw new System.Exception($"Statement '{statement.Kind}' is not resolved");
        }

        private ResolvedDeclareVariableStatement ResolveDeclareVariableStatement(SyntaxDeclareVariableStatement statement)
        {
            var keyword = statement.Keyword;
            var identifier = statement.Identifier;

            var type = statement.Keyword.Type.GetVariableType();
            if (type == ObjectTypes.Unknown)
                _diagnostic.ReportUndefinedTypeError(keyword.Value, keyword.Location);

            var variable = new Variable(identifier.Value, type);
            ResolvedExpression initExpression = null;

            if (statement.InitializedExpression != null)
                initExpression = ResolveExpression(statement.InitializedExpression);

            if (identifier.Value == null)
                return new ResolvedDeclareVariableStatement(null, initExpression, statement.Operator?.Location);

            if (_scope.ContainsKey(variable))
            {
                _diagnostic.ReportVariableAlreadyExistError(identifier.Value, identifier.Location);

                return new ResolvedDeclareVariableStatement(null, initExpression, statement.Operator?.Location);
            }

            _scope.Add(variable, null);

            return new ResolvedDeclareVariableStatement(variable, initExpression, statement.Operator?.Location);
        }

        #endregion
    }
}