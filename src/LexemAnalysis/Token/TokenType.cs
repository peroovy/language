namespace Translator
{
    internal enum TokenType
    {
        Number,
        Identifier,

        TrueKeyword,
        FalseKeyword,

        Plus,
        Minus,
        Star,
        StarStar,
        Slash,
        OpenParenthesis,
        CloseParenthesis,

        Space,
        LineSeparator,

        Unknown,
        EOF,
    }
}
