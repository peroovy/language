namespace Translator
{
    internal enum TokenType
    {
        Number,
        Identifier,

        Plus,
        Minus,
        Star,
        DoubleStar,
        Slash,
        OpenParenthesis,
        CloseParenthesis,

        LeftArrow,
        RightArrow,
        DoubleEquals,
        BangEquals,
        LeftArrowEquals,
        RightArrowEquals,
        DoubleOpersand,
        DoubleVerticalBar,
        Bang,

        Space,
        LineSeparator,

        TrueKeyword,
        FalseKeyword,

        Unknown,
        EOF,
    }
}
