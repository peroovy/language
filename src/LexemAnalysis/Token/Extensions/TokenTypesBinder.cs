using System.Collections.Generic;

namespace Translator
{
    internal static class TokenTypesBinder
    {
        private static readonly Dictionary<char, TokenTypes> _singleTerminals = 
            new Dictionary<char, TokenTypes>
        {
            ['+'] = TokenTypes.Plus,
            ['-'] = TokenTypes.Minus,
            ['*'] = TokenTypes.Star,
            ['/'] = TokenTypes.Slash,
            ['='] = TokenTypes.Equals,

            ['('] = TokenTypes.OpenParenthesis,
            [')'] = TokenTypes.CloseParenthesis,

            ['!'] = TokenTypes.Bang,
            ['<'] = TokenTypes.LeftArrow,
            ['>'] = TokenTypes.RightArrow,

            ['\0'] = TokenTypes.EOF,
        };

        private static readonly Dictionary<string, TokenTypes> _doubleTerminals =
            new Dictionary<string, TokenTypes>
        {
            ["**"] = TokenTypes.DoubleStar,

            ["=="] = TokenTypes.DoubleEquals,
            ["!="] = TokenTypes.BangEquals,
            ["<="] = TokenTypes.LeftArrowEquals,
            [">="] = TokenTypes.RightArrowEquals,

            ["+="] = TokenTypes.PlusEquals,
            ["-="] = TokenTypes.MinusEquals,
            ["*="] = TokenTypes.StarEquals,
            ["/="] = TokenTypes.SlashEquals,
            ["&="] = TokenTypes.OpersandEquals,
            ["|="] = TokenTypes.VerticalBarEquals,

            ["&&"] = TokenTypes.DoubleOpersand,
            ["||"] = TokenTypes.DoubleVerticalBar
        };

        private static readonly Dictionary<string, TokenTypes> _keywords =
            new Dictionary<string, TokenTypes>
        {
            ["int"] = TokenTypes.IntKeyword,
            ["float"] = TokenTypes.FloatKeyword,
            ["bool"] = TokenTypes.BoolKeyword,
            ["var"] = TokenTypes.VarKeyword,

            ["true"] = TokenTypes.TrueKeyword,
            ["false"] = TokenTypes.FalseKeyword
        };

        public static TokenTypes GetSingleType(this char sym) => GetType(sym, _singleTerminals);

        public static TokenTypes GetDoubleType(this string str) => GetType(str, _doubleTerminals);

        public static TokenTypes GetKeywordType(this string str) => GetType(str, _keywords);

        private static TokenTypes GetType<TKey>(TKey key, Dictionary<TKey, TokenTypes> bindings)
        {
            return bindings.TryGetValue(key, out var type) ? type : TokenTypes.Unknown;
        }
    }
}
