using System.Collections.Generic;
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

        public CompilationState<SyntaxExpression> Parse(SourceCode code, IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
            _diagnostic = new Diagnostic(code);

            var expession = ParseBinaryExpression();

            return new CompilationState<SyntaxExpression>(expession, _diagnostic.Errors);
        }

        private Token Current => _position < _tokens.Count ? _tokens[_position] : _tokens.Last();

        private Token NextToken()
        {
            var current = Current;
            _position += current.Type != TokenType.EOF ? 1 : 0;

            return current;
        }

        private Token MatchToken(TokenType type)
        {
            if (Current.Type == type)
                return NextToken();

            _diagnostic.ReportUnexpectedTokenError(Current.Type, type, Current.Location);

            return new Token(type, null, Current.Location);
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
                case TokenType.OpenParenthesis:
                {
                    var openParenthesis = NextToken();
                    var expression = ParseBinaryExpression();
                    var closeParenthesis = MatchToken(TokenType.CloseParenthesis);

                    return new SyntaxParenthesizedExpression(openParenthesis, expression, closeParenthesis);
                }

                case TokenType.TrueKeyword:
                case TokenType.FalseKeyword:
                {
                    var value = NextToken();

                    return new SyntaxLiteralExpression(value, ObjectTypes.Bool);
                }
            }

            var number = MatchToken(TokenType.Number);
            var type = number.Value == null 
                ? ObjectTypes.Unknown 
                : number.Value.Contains('.') ? ObjectTypes.Float : ObjectTypes.Int;

            return new SyntaxLiteralExpression(number, type);
        }
    }
}
