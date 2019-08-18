namespace Highlight.Patterns
{
    using System;
    using System.Text.RegularExpressions;

    public sealed class BlockPattern : Pattern
    {
        public BlockPattern(string name, Style style, string beginsWith, string endsWith, string escapesWith)
        : base(name, style)
        {
            BeginsWith = beginsWith;
            EndsWith = endsWith;
            EscapesWith = escapesWith;
        }

        public string BeginsWith { get; }
        public string EndsWith { get; }
        public string EscapesWith { get; }

        public static string Escape(string str)
        {
            if (!string.Equals(str, @"\n", StringComparison.Ordinal))
                str = Regex.Escape(str);

            return str;
        }

        public override string GetRegexPattern()
            => string.IsNullOrEmpty(EscapesWith)
            ? string.Equals(EndsWith, @"\n", StringComparison.Ordinal) ? $@"{Escape(BeginsWith)}[^\n\r]*" : $@"{Escape(BeginsWith)}[\w\W\s\S]*?{Escape(EndsWith)}"
            : $"{Regex.Escape(BeginsWith)}(?>{Regex.Escape(EscapesWith.Substring(0, 1))}.|[^{Regex.Escape(EndsWith.Substring(0, 1))}]|.)*?{Regex.Escape(EndsWith)}";
    }
}