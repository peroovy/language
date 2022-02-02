namespace Translator
{
    internal class Error
    {
        public Error(string message, string code, TextSpan span)
        {
            Message = message;
            Code = code;
            Span = span;
        }

        public string Message { get; }
        public string Code { get; }
        public TextSpan Span { get; }
    }
}
