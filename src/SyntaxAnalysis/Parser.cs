﻿using System.Collections.Generic;
using System.Linq;
using Translator.AST;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Parser
    {
        private int _position;
        private IReadOnlyList<Token> _tokens;

        private Diagnostic _diagnostic;

        private Token Current => Peek(0);

        public CompilationState<SyntaxNode> Parse(SourceCode code, IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
            _diagnostic = new Diagnostic(code);

            SyntaxNode node = null;

            if (Current.Type == TokenTypes.IntKeyword 
                || Current.Type == TokenTypes.FloatKeyword 
                || Current.Type == TokenTypes.BoolKeyword)
            {
                node = ParseStatement();
            }
            else
            {
                node = ParseExpression();
            }

            return new CompilationState<SyntaxNode>(node, _diagnostic.Errors);
        }

        private Token Peek(int offset)
        {
            return _position + offset < _tokens.Count 
                ? _tokens[_position + offset] 
                : _tokens.Last();
        }

        private Token NextToken()
        {
            var current = Current;
            _position += current.Type != TokenTypes.EOF ? 1 : 0;

            return current;
        }

        private Token MatchToken(TokenTypes type)
        {
            if (Current.Type == type)
                return NextToken();

            _diagnostic.ReportUnexpectedTokenError(Current.Type, type, Current.Location);

            return new Token(type, null, Current.Location);
        }

        #region Expressions

        private SyntaxExpression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private SyntaxExpression ParseAssignmentExpression()
        {
            if (Peek(0).Type == TokenTypes.Identifier 
                && Peek(1).Type == TokenTypes.Equals)
            {
                var identifier = NextToken();
                var op = NextToken();
                var expression = ParseExpression();

                return new SyntaxAssignmentExpression(identifier, op, expression);
            }

            return ParseBinaryExpression();
        }

        private SyntaxExpression ParseBinaryExpression(int parentPrecedence = 0)
        {
            SyntaxExpression left;

            var unaryPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryPrecedence != null && unaryPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var expression = ParseBinaryExpression(unaryPrecedence.Value);

                left = new SyntaxUnaryExpression(operatorToken, expression);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Type.GetBinaryOperatorPrecedence();
                if (precedence is null || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence.Value);

                left = new SyntaxBinaryExpression(left, operatorToken, right);
            }

            return left;
        }

        private SyntaxExpression ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case TokenTypes.OpenParenthesis:
                {
                    var openParenthesis = NextToken();
                    var expression = ParseExpression();
                    var closeParenthesis = MatchToken(TokenTypes.CloseParenthesis);

                    return new SyntaxParenthesizedExpression(openParenthesis, expression, closeParenthesis);
                }

                case TokenTypes.TrueKeyword:
                case TokenTypes.FalseKeyword:
                {
                    var value = NextToken();

                    return new SyntaxLiteralExpression(value, ObjectTypes.Bool);
                }

                case TokenTypes.Identifier:
                {
                    var name = NextToken();

                    return new SyntaxIdentifierExpression(name);
                }
            }

            var number = MatchToken(TokenTypes.Number);
            var type = number.Value == null 
                ? ObjectTypes.Unknown 
                : number.Value.Contains('.') ? ObjectTypes.Float : ObjectTypes.Int;

            return new SyntaxLiteralExpression(number, type);
        }

        #endregion

        #region Statement

        private SyntaxStatement ParseStatement()
        {
            return ParseVariableDeclarationStatement();
        }

        private SyntaxVariableDeclarationStatement ParseVariableDeclarationStatement()
        {
            var keyword = NextToken();
            var identifier = MatchToken(TokenTypes.Identifier);

            if (Current.Type != TokenTypes.Equals)
                return new SyntaxVariableDeclarationStatement(keyword, identifier);

            var op = NextToken();
            var expression = ParseBinaryExpression();

            return new SyntaxVariableDeclarationStatement(keyword, identifier, op, expression);
        }

        #endregion
    }
}
