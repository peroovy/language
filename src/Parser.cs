using System.Collections.Generic;
using System.Linq;
using Translator.AST;

namespace Translator
{
    internal sealed class Parser
    {
        private List<Token> _tokens;
        private int _position;

        private readonly List<string> _errors = new List<string>();

        public Expression Parse(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _errors.Clear();

            return ParseBinaryExpression();
        }

        // TODO: temp
        public IEnumerable<string> Errors => _errors;

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

            _errors.Add($"ERROR: Expected '{type}', but was '{Current.Type}' in line {Current.NumberLine} on position {Current.Position}");

            return new Token(type, null, Current.Position, Current.NumberLine);
        }

        private Expression ParseBinaryExpression(int parentPrecedence = 0)
        {
            Expression left;

            if (Current.Type.IsUnaryOperator())
            {
                var operatorToken = NextToken();
                var expression = ParseBinaryExpression(parentPrecedence + 1);

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
            }

            var number = MatchToken(TokenType.Number);
            return new LiteralExpression(number);
        }
    }
}
