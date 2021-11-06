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

        public static TokenType ToTokenType(this char sym)
        {
            if (_terminals.TryGetValue(sym, out var type))
                return type;

            return TokenType.Unknown;
        }
    }
}
