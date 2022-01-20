namespace Translator
{
    internal enum TokenTypes
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
        Equals,
        DoubleEquals,
        BangEquals,
        LeftArrowEquals,
        RightArrowEquals,
        DoubleOpersand,
        DoubleVerticalBar,
        Bang,

        Space,
        LineSeparator,

        IntKeyword,
        FloatKeyword,
        BoolKeyword,
        TrueKeyword,
        FalseKeyword,

        Unknown,
        EOF,
    }
}
