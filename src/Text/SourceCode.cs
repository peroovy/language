using System.Collections.Generic;

namespace Translator
{
    internal class SourceCode
    {
        public SourceCode(string code)
        {
            Text = code;
            Lines = Text.Split('\n');
        }

        public string Text { get; }
        public IReadOnlyList<string> Lines { get; }
    }
}
