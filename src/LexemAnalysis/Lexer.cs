using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Translator
{
    internal sealed class Lexer
    {
        private readonly Regex _numberRegex = new Regex(@"([0-9]*[.])?[0-9]+");
        private readonly Regex _wordRegex = new Regex(@"_*([a-z]|[A-Z]|[а-я]|[А-Я])\w*");

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

        private Token NextToken()
        {
            if (char.IsDigit(Current) || Current == '.' && char.IsDigit(Peek(1)))
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
            var number = _numberRegex
                .Match(_code, _position)
                .ToString();

            _position += number.Length;

            return new Token(TokenType.Number, number, _positionInLine, _numberLine);
        }

        private Token NextWordToken()
        {
            var word = _wordRegex
                .Match(_code, _position)
                .ToString();

            var type = word.GetKeywordType();
            if (type == TokenType.Unknown)
                type = TokenType.Identifier;

            _position += word.Length;

            return new Token(type, word, _positionInLine, _numberLine);
        }

        private Token NextWhitespaceToken()
        {
            if (Current == '\n')
            {
                _position++;
                return new Token(TokenType.LineSeparator, "\n", _positionInLine, _numberLine);
            }

            var value = new string(_code
                    .TakeWhile(_position, sym => char.IsWhiteSpace(sym) && sym != '\n')
                    .ToArray());

            _position += value.Length;

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
