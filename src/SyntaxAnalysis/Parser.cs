using System.Collections.Generic;
using System.Linq;
using Translator.AST;

namespace Translator
{
    internal sealed class Parser
    {
        private int _position;
        private readonly IReadOnlyList<Token> _tokens;

        private readonly Diagnostic _diagnostic;

        public Parser(SourceCode code, IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
            _diagnostic = new Diagnostic(code);
        }

        public IEnumerable<Error> Errors => _diagnostic.Errors;

        public Expression Parse()
        {
            return ParseBinaryExpression();
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

        private Expression ParseBinaryExpression(int parentPrecedence = 0)
        {
            Expression left;

            var unaryPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryPrecedence != null && unaryPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var expression = ParseBinaryExpression(unaryPrecedence.Value);

                left = new UnaryExpression(operatorToken, expression);
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

                left = new BinaryExpression(left, operatorToken, right);
            }

            return left;
        }

        private Expression ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case TokenType.OpenParenthesis:
                {
                    var openParenthesis = NextToken();
                    var expression = ParseBinaryExpression();
                    var closeParenthesis = MatchToken(TokenType.CloseParenthesis);

                    return new ParenthesizedExpression(openParenthesis, expression, closeParenthesis);
                }

                case TokenType.TrueKeyword:
                case TokenType.FalseKeyword:
                {
                    var boolean = NextToken();

                    return new LiteralExpression(boolean);
                }
            }

            var number = MatchToken(TokenType.Number);
            return new LiteralExpression(number);
        }
    }
}
