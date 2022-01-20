namespace Translator
{
    internal sealed class Token
    {
        public Token(TokenTypes type, string value, int positionInLine, int numberLine)
            : this(type, value, new TextLocation(positionInLine, value.Length, numberLine))
        {
        }

        public Token(TokenTypes type, string value, TextLocation location)
        {
            Type = type;
            Value = value;
            Location = location;
        }

        public TokenTypes Type { get; }
        public string Value { get; }
        public TextLocation Location { get; }
    }
}
