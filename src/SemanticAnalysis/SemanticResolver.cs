using System.Collections.Generic;
using Translator.AST;
using Translator.ObjectModel;
using Translator.SRT;

namespace Translator
{
    internal sealed class SemanticResolver
    {
        private Diagnostic _diagnostic;
        private Dictionary<string, Variable> _scope;

        public CompilationState<ResolvedNode> Resolve(SourceCode code, SyntaxNode node, Dictionary<string, Variable> scope)
        {
            _diagnostic = new Diagnostic(code);
            _scope = scope;

            switch (node.Kind)
            {
                case SyntaxNodeKind.VariableDeclarationStatement:
                {
                    var statement = ResolveStatement((SyntaxStatement)node);

                    return new CompilationState<ResolvedNode>(statement, _diagnostic.Errors);
                }
            }

            var expression = ResolveExpression((SyntaxExpression)node);

            return new CompilationState<ResolvedNode>(expression, _diagnostic.Errors);
        }

        #region Expressions

        private ResolvedExpression ResolveExpression(SyntaxExpression expression)
        {
            switch (expression.Kind)
            {
                case SyntaxNodeKind.LiteralExpression:
                    return ResolveLiteralExpression((SyntaxLiteralExpression)expression);

                case SyntaxNodeKind.IdentifierExpression:
                    return ResolveIdentifierExpression((SyntaxIdentifierExpression)expression);

                case SyntaxNodeKind.ParenthesizedExpression:
                    return ResolveParenthesizedExpression((SyntaxParenthesizedExpression)expression);

                case SyntaxNodeKind.UnaryExpression:
                    return ResolveUnaryExpression((SyntaxUnaryExpression)expression);

                case SyntaxNodeKind.BinaryExpression:
                    return ResolveBinaryExpression((SyntaxBinaryExpression)expression);

                case SyntaxNodeKind.AssignmentExpression:
                    return ResolveAssignmentExpression((SyntaxAssignmentExpression)expression);
            }

            throw new System.Exception($"Expression '{expression.Kind}' is not resolved");
        }

        private ResolvedLiteralExpression ResolveLiteralExpression(SyntaxLiteralExpression literal)
        {
            return new ResolvedLiteralExpression(literal.Token.Value, literal.ObjectType);
        }

        private ResolvedExpression ResolveIdentifierExpression(SyntaxIdentifierExpression expression)
        {
            Token identifier = expression.Name;

            if (!_scope.TryGetValue(identifier.Value, out Variable variable))
            {
                _diagnostic.ReportUndefinedVariableError(identifier.Value, identifier.Location);

                return new ResolvedLostExpression();
            }

            return new ResolvedIdentifierExpression(variable);
        }

        private ResolvedParenthesizedExpression ResolveParenthesizedExpression(SyntaxParenthesizedExpression parentheses)
        {
            var expression = ResolveExpression(parentheses.Expression);

            return new ResolvedParenthesizedExpression(expression);
        }

        private ResolvedExpression ResolveUnaryExpression(SyntaxUnaryExpression unary)
        {
            IUnaryOperation operation = unary.OperatorToken.Type.ToUnaryOperation();
            var operand = ResolveExpression(unary.Operand);

            if (operation is null)
                throw new System.Exception($"The unary operator '{unary.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(operand.Type))
                return new ResolvedUnaryExpression(operation, operand);

            _diagnostic.ReportUndefinedUnaryOperationForType(operation.Kind, operand.Type, unary.OperatorToken.Location);

            return new ResolvedLostExpression();
        }

        private ResolvedExpression ResolveBinaryExpression(SyntaxBinaryExpression binary)
        {
            var left = ResolveExpression(binary.Left);
            var right = ResolveExpression(binary.Right);
            IBinaryOperation operation = binary.OperatorToken.Type.ToBinaryOperation();

            if (operation is null)
                throw new System.Exception($"The binary operator '{binary.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(left.Type, right.Type))
                return new ResolvedBinaryExpression(left, operation, right, binary.OperatorToken.Location);

            _diagnostic.ReportUndefinedBinaryOperationForTypes(left.Type, operation.Kind, right.Type, binary.OperatorToken.Location);

            return new ResolvedLostExpression();
        } 

        private ResolvedExpression ResolveAssignmentExpression(SyntaxAssignmentExpression expression)
        {
            var value = ResolveExpression(expression.Expression);

            if (!_scope.TryGetValue(expression.Identifier.Value, out var variable))
            {
                _diagnostic.ReportUndefinedVariableError(expression.Identifier.Value, expression.Identifier.Location);

                return new ResolvedLostExpression();
            }

            if (!ImplicitCast.Instance.IsApplicable(value.Type, variable.Type))
            {
                _diagnostic.ReportImpossibleImplicitCast(value.Type, variable.Type, expression.Operator.Location);

                return new ResolvedLostExpression();
            }

            return new ResolvedAssignmentExpression(variable, value, expression.Operator.Location);
        }

        #endregion

        #region Statements

        private ResolvedStatement ResolveStatement(SyntaxStatement statement)
        {
            switch (statement.Kind)
            {
                case SyntaxNodeKind.VariableDeclarationStatement:
                    return ResolveVariableDeclarationStatement((SyntaxVariableDeclarationStatement)statement);
            }

            throw new System.Exception($"Statement '{statement.Kind}' is not resolved");
        }

        private ResolvedStatement ResolveVariableDeclarationStatement(SyntaxVariableDeclarationStatement statement)
        {
            Token keyword = statement.Keyword;
            Token identifier = statement.Identifier;

            ObjectTypes type = statement.Keyword.Type.GetVariableType();
            if (type == ObjectTypes.Unknown)
                _diagnostic.ReportUndefinedTypeError(keyword.Value, keyword.Location);

            var initExpression = statement.InitializedExpression != null
                ? ResolveExpression(statement.InitializedExpression)
                : null;

            if (identifier.Value == null)
                return new ResolvedLostStatement();

            if (_scope.ContainsKey(identifier.Value))
            {
                _diagnostic.ReportVariableAlreadyExistError(identifier.Value, identifier.Location);

                return new ResolvedLostStatement();
            }

            if (initExpression != null && !ImplicitCast.Instance.IsApplicable(initExpression.Type, type))
            {
                _diagnostic.ReportImpossibleImplicitCast(initExpression.Type, type, statement.Operator.Location);

                return new ResolvedLostStatement();
            }

            _scope[identifier.Value] = new Variable(identifier.Value, type);

            return new ResolvedVariableDeclarationStatement(_scope[identifier.Value], initExpression, statement.Operator?.Location);
        }

        #endregion
    }
}