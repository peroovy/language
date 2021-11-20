using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator
{
    internal sealed class Lexer
    {
        private string _code;
        private int _position;

        private int _positionInLine;
        private int _numberLine;

        private Diagnostic _diagnostic;

        public CompilationState<IReadOnlyList<Token>> Tokenize(SourceCode code)
        {
            _diagnostic = new Diagnostic(code);
            _code = code.Text;
            _positionInLine = 0;
            _numberLine = 0;

            var tokens = new List<Token>();

            Token token;
            do
            {
                token = NextToken();

                if (token.Type == TokenType.Unknown)
                    _diagnostic.ReportUnknownTokenError(token.Value, token.Location);

                if (token.Type == TokenType.LineSeparator)
                {
                    _positionInLine = 0;
                    _numberLine++;
                }
                else
                {
                    _positionInLine += token.Value.Length;
                }

                if (token.Type != TokenType.Space
                    && token.Type != TokenType.LineSeparator)
                {
                    tokens.Add(token);
                }

            } while (token.Type != TokenType.EOF);

            return new CompilationState<IReadOnlyList<Token>>(tokens, _diagnostic.Errors);
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

        private Token NextToken()
        {
            if (char.IsDigit(Current))
                return NextNumberToken();

            if (char.IsLetter(Current) || Current == '_')
                return NextWordToken();

            if (char.IsWhiteSpace(Current))
                return NextWhitespaceToken();

            if (TryNextDoubleToken(out var token))
                return token;

            token = new Token(Current.GetSingleType(), Current.ToString(), _positionInLine, _numberLine);
            _position++;

            return token;
        }

        private Token NextNumberToken()
        {
            var value = NextValue(char.IsDigit);

            return new Token(TokenType.Number, value, _positionInLine, _numberLine);
        }

        private Token NextWordToken()
        {
            var value = NextValue(sym => char.IsLetter(sym) || sym == '_');

            var type = value.GetKeywordType();
            if (type == TokenType.Unknown)
                type = TokenType.Identifier;

            return new Token(type, value, _positionInLine, _numberLine);
        }

        private Token NextWhitespaceToken()
        {
            if (Current == '\n')
            {
                _position++;
                return new Token(TokenType.LineSeparator, "\n", _positionInLine, _numberLine);
            }

            var value = NextValue(sym => char.IsWhiteSpace(sym) && sym != '\n');

            return new Token(TokenType.Space, value, _positionInLine, _numberLine);
        }

        private bool TryNextDoubleToken(out Token token)
        {
            var value = new string(new[] { Peek(0), Peek(1) });
            var type = value.GetDoubleType();
            token = new Token(type, value, _positionInLine, _numberLine);

            if (type != TokenType.Unknown)
            {
                _position += 2;
                return true;
            }

            return false;
        }
    }
}
