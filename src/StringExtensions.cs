using System;
using System.Collections.Generic;

namespace Translator
{
    internal static class StringExtensions
    {
        public static IEnumerable<char> TakeWhile(this string str, int start, Func<char, bool> predicate)
        {
            while (start < str.Length && predicate(str[start]))
                yield return str[start++];
        }
    }
}
