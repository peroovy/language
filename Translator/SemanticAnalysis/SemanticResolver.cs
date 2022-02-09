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

                case SyntaxNodeKind.ExplicitCastExpression:
                    return ResolveExplicitCastExpression((SyntaxExplicitCastExpression)expression);
            }

            throw new System.Exception($"Expression '{expression.Kind}' is not resolved");
        }

        private ResolvedLiteralExpression ResolveLiteralExpression(SyntaxLiteralExpression syntax)
        {
            return new ResolvedLiteralExpression(syntax.Token.Value, syntax.ObjectType);
        }

        private ResolvedExpression ResolveIdentifierExpression(SyntaxIdentifierExpression syntax)
        {
            Token identifier = syntax.Name;

            if (!_scope.TryGetValue(identifier.Value, out Variable variable))
            {
                _diagnostic.ReportUndefinedVariableError(identifier.Value, identifier.Location);

                return new ResolvedLostExpression();
            }

            return new ResolvedIdentifierExpression(variable);
        }

        private ResolvedParenthesizedExpression ResolveParenthesizedExpression(SyntaxParenthesizedExpression syntax)
        {
            var expression = ResolveExpression(syntax.Expression);

            return new ResolvedParenthesizedExpression(expression);
        }

        private ResolvedExpression ResolveUnaryExpression(SyntaxUnaryExpression syntax)
        {
            UnaryOperation operation = syntax.OperatorToken.Type.ToUnaryOperation();
            var operand = ResolveExpression(syntax.Operand);

            if (operation is null)
                throw new System.Exception($"The unary operator '{syntax.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(operand.Type))
                return new ResolvedUnaryExpression(operation, operand);

            _diagnostic.ReportUndefinedUnaryOperationForType(operation.Kind, operand.Type, syntax.OperatorToken.Location);

            return new ResolvedLostExpression();
        }

        private ResolvedExpression ResolveBinaryExpression(SyntaxBinaryExpression syntax)
        {
            var left = ResolveExpression(syntax.Left);
            var right = ResolveExpression(syntax.Right);
            BinaryOperation operation = syntax.OperatorToken.Type.ToBinaryOperation();

            if (operation is null)
                throw new System.Exception($"The binary operator '{syntax.OperatorToken.Value}' is not resolved");

            if (operation.IsApplicable(left.Type, right.Type))
                return new ResolvedBinaryExpression(left, operation, right, syntax.OperatorToken.Location);

            _diagnostic.ReportUndefinedBinaryOperationForTypes(left.Type, operation.Kind, right.Type, syntax.OperatorToken.Location);

            return new ResolvedLostExpression();
        } 

        private ResolvedExpression ResolveAssignmentExpression(SyntaxAssignmentExpression syntax)
        {
            var value = ResolveExpression(syntax.Expression);

            ObjectTypes expressionType = value.Type;
            BinaryOperation operation = syntax.Operator.Type.ToBinaryOperation();

            if (!_scope.TryGetValue(syntax.Identifier.Value, out var variable))
            {
                _diagnostic.ReportUndefinedVariableError(syntax.Identifier.Value, syntax.Identifier.Location);

                return new ResolvedLostExpression();
            }

            if (operation != null)
            {
                if (!operation.IsApplicable(variable.Type, value.Type))
                {
                    _diagnostic.ReportUndefinedBinaryOperationForTypes(
                        variable.Type, operation.Kind, value.Type, syntax.Operator.Location);

                    return new ResolvedLostExpression();
                }

                expressionType = operation.GetObjectType(variable.Type, value.Type);
            }

            if (!ImplicitCasting.Instance.IsApplicable(expressionType, variable.Type))
            {
                _diagnostic.ReportImpossibleImplicitCast(value.Type, variable.Type, syntax.Operator.Location);

                return new ResolvedLostExpression();
            }

            return new ResolvedAssignmentExpression(variable, value, operation, syntax.Operator.Location);
        }

        private ResolvedExpression ResolveExplicitCastExpression(SyntaxExplicitCastExpression syntax)
        {
            ObjectTypes target = syntax.Keyword.Type.GetObjectType();
            var expression = ResolveExpression(syntax.Expression);

            if (!ExplicitCasting.Instance.IsApplicable(expression.Type, target))
            {
                _diagnostic.ReportImpossibleExplicitCast(expression.Type, target, syntax.Keyword.Location);

                return new ResolvedLostExpression();
            }

            return new ResolvedExplicitCastExpression(expression, target);
        }

        #endregion

        #region Statements

        private ResolvedStatement ResolveStatement(SyntaxStatement syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxNodeKind.VariableDeclarationStatement:
                    return ResolveVariableDeclarationStatement((SyntaxVariableDeclarationStatement)syntax);
            }

            throw new System.Exception($"Statement '{syntax.Kind}' is not resolved");
        }

        private ResolvedStatement ResolveVariableDeclarationStatement(SyntaxVariableDeclarationStatement syntax)
        {
            Token keyword = syntax.Keyword;
            Token identifier = syntax.Identifier;
            var initExpression = syntax.InitializedExpression != null
                ? ResolveExpression(syntax.InitializedExpression)
                : null;

            var type = keyword.Type == TokenTypes.VarKeyword
                ? initExpression.Type 
                : keyword.Type.GetObjectType();
            if (type == ObjectTypes.Unknown)
            {
                _diagnostic.ReportUndefinedTypeError(keyword.Value, keyword.Location);

                return new ResolvedLostStatement();
            }

            if (identifier.Value == null)
                return new ResolvedLostStatement();

            if (_scope.ContainsKey(identifier.Value))
            {
                _diagnostic.ReportVariableAlreadyExistError(identifier.Value, identifier.Location);

                return new ResolvedLostStatement();
            }

            if (initExpression != null && !ImplicitCasting.Instance.IsApplicable(initExpression.Type, type))
            {
                _diagnostic.ReportImpossibleImplicitCast(initExpression.Type, type, syntax.Operator.Location);

                return new ResolvedLostStatement();
            }

            _scope[identifier.Value] = new Variable(identifier.Value, type);

            return new ResolvedVariableDeclarationStatement(_scope[identifier.Value], initExpression, syntax.Operator?.Location);
        }

        #endregion
    }
}