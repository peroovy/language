namespace Translator
{
    internal sealed class Token
    {
        public Token(TokenType type, string value, int position, int numberLine)
        {
            Type = type;
            Value = value;
            Position = position;
            NumberLine = numberLine;
        }

        public TokenType Type { get; }
        public string Value { get; }
        public int Position { get; }
        public int NumberLine { get; }
    }
}
