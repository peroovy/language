using System.Collections.Generic;

namespace Translator
{
    internal static class TokenInfo
    {
        private static readonly Dictionary<char, TokenType> _terminals = 
            new Dictionary<char, TokenType>
        {
            ['+'] = TokenType.Plus,
            ['-'] = TokenType.Minus,
            ['*'] = TokenType.Star,
            ['/'] = TokenType.Slash,
            ['('] = TokenType.OpenParenthesis,
            [')'] = TokenType.CloseParenthesis,
            ['\0'] = TokenType.EOF,
        };

        private static readonly Dictionary<string, TokenType> _keywords =
            new Dictionary<string, TokenType>
            {
                ["true"] = TokenType.TrueKeyword,
                ["false"] = TokenType.FalseKeyword
            };

        public static TokenType GetSingleTerminal(this char sym)
        {
            if (_terminals.TryGetValue(sym, out var type))
                return type;

            return TokenType.Unknown;
        }

        public static TokenType GetKeywordType(this string str)
        {
            if (_keywords.TryGetValue(str, out var type))
                return type;

            return TokenType.Unknown;
        }
    }
}
