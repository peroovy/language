namespace Translator
{
    internal sealed class Token
    {
        public Token(TokenType type, string value, int positionInLine, int numberLine)
            : this(type, value, new TextLocation(positionInLine, value.Length, numberLine))
        {
        }

        public Token(TokenType type, string value, TextLocation location)
        {
            Type = type;
            Value = value;
            Location = location;
        }

        public TokenType Type { get; }
        public string Value { get; }
        public TextLocation Location { get; }
    }
}
