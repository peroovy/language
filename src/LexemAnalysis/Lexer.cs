using System;
using System.Collections.Generic;
using System.Linq;

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

        private string NextValue(Func<char, bool> predicate)
        {
            var value = new string(_code
                    .TakeWhile(_position, predicate)
                    .ToArray());

            _position += value.Length;

            return value;
        }

        private Token NextToken(int positionInLine, int numberLine)
        {
            if (char.IsDigit(Current))
                return NextNumberToken(positionInLine, numberLine);

            if (char.IsLetter(Current) || Current == '_')
                return NextWordToken(positionInLine, numberLine);

            if (char.IsWhiteSpace(Current))
                return NextWhitespaceToken(positionInLine, numberLine);

            if (TryNextDoubleToken(positionInLine, numberLine, out var token))
                return token;

            token = new Token(Current.GetSingleType(), Current.ToString(), positionInLine, numberLine);
            _position++;

            return token;
        }

        private Token NextNumberToken(int positionInLine, int numberLine)
        {
            var value = NextValue(char.IsDigit);

            return new Token(TokenType.Number, value, positionInLine, numberLine);
        }

        private Token NextWordToken(int positionInLine, int numberLine)
        {
            var value = NextValue(sym => char.IsLetter(sym) || sym == '_');

            var type = value.GetKeywordType();
            if (type == TokenType.Unknown)
                type = TokenType.Identifier;

            return new Token(type, value, positionInLine, numberLine);
        }

        private Token NextWhitespaceToken(int positionInLine, int numberLine)
        {
            if (Current == '\n')
            {
                _position++;
                return new Token(TokenType.LineSeparator, "\n", positionInLine, numberLine);
            }

            var value = NextValue(sym => char.IsWhiteSpace(sym) && sym != '\n');

            return new Token(TokenType.Space, value, positionInLine, numberLine);
        }

        private bool TryNextDoubleToken(int positionInLine, int numberLine, out Token token)
        {
            var value = new string(new[] { Peek(0), Peek(1) });
            var type = value.GetDoubleType();
            token = new Token(type, value, positionInLine, numberLine);

            if (type != TokenType.Unknown)
            {
                _position += 2;
                return true;
            }

            return false;
        }
    }
}
