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

        PlusEquals,
        MinusEquals,
        StarEquals,
        SlashEquals,
        OpersandEquals,
        VerticalBarEquals,

        Space,
        LineSeparator,

        IntKeyword,
        FloatKeyword,
        BoolKeyword,
        LongKeyword,
        VarKeyword,

        TrueKeyword,
        FalseKeyword,

        Unknown,
        EOF,
    }
}
