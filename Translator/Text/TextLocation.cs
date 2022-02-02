namespace Translator
{
    internal struct TextLocation
    {  
        public TextLocation(int positionInLine, int length, int numberLine)
        {
            NumberLine = numberLine;
            Span = new TextSpan(positionInLine, length);
        }

        public int NumberLine { get; }
        public TextSpan Span { get; }
    }
}
