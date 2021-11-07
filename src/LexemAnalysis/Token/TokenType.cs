namespace Translator
{
    internal enum TokenType
    {
        Number,
        Identifier,

        TrueKeyword,
        FalseKeyword,
        DoubleOpersand,
        DoubleVerticalBar,
        Bang,

        Plus,
        Minus,
        Star,
        DoubleStar,
        Slash,
        OpenParenthesis,
        CloseParenthesis,

        Space,
        LineSeparator,

        Unknown,
        EOF,
    }
}
