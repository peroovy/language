using System;
using System.Collections.Generic;

namespace Translator
{
    internal sealed class Lexer
    {
        private readonly Diagnostic _diagnostic;

        private int _position;
        private readonly string _code;

        public Lexer(SourceCode code)
        {
            _code = code.Text;
            _diagnostic = new Diagnostic(code);
        }

        public IEnumerable<Error> Errors => _diagnostic.Errors;

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();
            var positionInLine = 0;
            var numberLine = 0;

            Token token;
            do
            {
                token = NextToken(positionInLine, numberLine);

                if (token.Type == TokenType.Unknown)
                    _diagnostic.ReportUnknownTokenError(token.Value, token.Location);

                if (token.Type == TokenType.LineSeparator)
                {
                    positionInLine = 0;
                    numberLine++;
                }
                else
                {
                    positionInLine += token.Value.Length;
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

        private Token NextToken(int positionInLine, int numberLine)
        {
            if (char.IsDigit(Current))
                return NextСoncatenatedToken(char.IsDigit, TokenType.Number, positionInLine, numberLine);

            if (char.IsWhiteSpace(Current))
            {
                if (Current == '\n')
                {
                    _position++;
                    return new Token(TokenType.LineSeparator, "\n", positionInLine, numberLine);
                }

                return NextСoncatenatedToken(
                    character => char.IsWhiteSpace(character) && character != '\n',
                    TokenType.Space, positionInLine, numberLine);
            }

            if (Current == '*' && Peek(1) == '*')
            {
                _position += 2;
                return new Token(TokenType.StarStar, "**", positionInLine, numberLine);
            }

            var terminal = new Token(Current.ToTokenType(), Current.ToString(), positionInLine, numberLine);
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
