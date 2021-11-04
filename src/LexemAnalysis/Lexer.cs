using System;
using System.Collections.Generic;

namespace Translator
{
    internal sealed class Lexer
    {
        private string _code;
        private int _position;

        private readonly List<string> _errors = new List<string>();

        private Dictionary<char, TokenType> _terminals = new Dictionary<char, TokenType>
        {
            ['+'] = TokenType.Plus,
            ['-'] = TokenType.Minus,
            ['*'] = TokenType.Star,
            ['/'] = TokenType.Slash,
            ['('] = TokenType.OpenParenthesis,
            [')'] = TokenType.CloseParenthesis,
            ['\0'] = TokenType.EOF,
        };

        // TODO: temp
        public IEnumerable<string> Errors => _errors;

        public List<Token> Tokenize(string code)
        {
            _code = code;
            _position = 0;
            _errors.Clear();

            var tokens = new List<Token>();
            var index = 0;
            var numberLine = 1;

            Token token;
            do
            {
                token = NextToken(index, numberLine);

                if (token.Type == TokenType.Unknown)
                    _errors.Add($"ERROR: Unknown token '{token.Value}' in line {token.NumberLine} on position {token.Position}");

                if (token.Type == TokenType.LineSeparator)
                {
                    index = 0;
                    numberLine++;
                }
                else
                {
                    index += token.Value.Length;
                }

                if (token.Type != TokenType.Space
                    && token.Type != TokenType.LineSeparator)
                {
                    tokens.Add(token);
                }

            } while (token.Type != TokenType.EOF);

            return tokens;
        }

        private char Current => Peek(0);

        private char Peek(int offset)
        {
            var index = _position + offset;

            return index < _code.Length ? _code[index] : '\0';
        }

        private Token NextToken(int index, int numberLine)
        {
            if (char.IsDigit(Current))
                return NextСoncatenatedToken(char.IsDigit, TokenType.Number, index, numberLine);

            if (char.IsWhiteSpace(Current))
            {
                if (Current == '\n')
                {
                    _position++;
                    return new Token(TokenType.LineSeparator, "\n", index, numberLine);
                }

                return NextСoncatenatedToken(character => char.IsWhiteSpace(character) && character != '\n',
                    TokenType.Space, index, numberLine);
            }

            if (Current == '*' && Peek(1) == '*')
            {
                _position += 2;
                return new Token(TokenType.StarStar, "**", index, numberLine);
            }


            var terminal = _terminals.TryGetValue(Current, out var type) 
                ? new Token(type, Current.ToString(), index, numberLine)
                : new Token(TokenType.Unknown, Current.ToString(), index, numberLine);

            _position++;
            return terminal;
        }

        private Token NextСoncatenatedToken(
            Func<char, bool> predicate,
            TokenType type, int index, int numberLine)
        {
            var start = _position;

            while (predicate(Current))
                _position++;

            var length = _position - start;

            return new Token(type, _code.Substring(start, length), index, numberLine);
        }
    }
}
