using System.Collections.Generic;

namespace Translator
{
    internal static class TokenTypesBinder
    {
        private static readonly Dictionary<char, TokenType> _singleTerminals = 
            new Dictionary<char, TokenType>
        {
            ['+'] = TokenType.Plus,
            ['-'] = TokenType.Minus,
            ['*'] = TokenType.Star,
            ['/'] = TokenType.Slash,
            ['('] = TokenType.OpenParenthesis,
            [')'] = TokenType.CloseParenthesis,
            ['!'] = TokenType.Bang,
            ['\0'] = TokenType.EOF,
        };

        private static readonly Dictionary<string, TokenType> _doubleTerminals =
            new Dictionary<string, TokenType>
        {
            ["**"] = TokenType.DoubleStar,
            ["&&"] = TokenType.DoubleOpersand,
            ["||"] = TokenType.DoubleVerticalBar
        };

        private static readonly Dictionary<string, TokenType> _keywords =
            new Dictionary<string, TokenType>
        {
            ["true"] = TokenType.TrueKeyword,
            ["false"] = TokenType.FalseKeyword
        };

        public static TokenType GetSingleType(this char sym) => GetType(sym, _singleTerminals);

        public static TokenType GetDoubleType(this string str) => GetType(str, _doubleTerminals);

        public static TokenType GetKeywordType(this string str) => GetType(str, _keywords);

        private static TokenType GetType<TKey>(TKey key, Dictionary<TKey, TokenType> bindings)
        {
            if (bindings.TryGetValue(key, out var type))
                return type;

            return TokenType.Unknown;
        }
    }
}
