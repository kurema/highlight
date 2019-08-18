namespace Highlight.Patterns
{
    using System.Collections.Generic;
    using System.Text;

    public class Definition
    {
        public Definition(string name, bool caseSensitive, Style style, IDictionary<string, Pattern> patterns)
        {
            Name = name;
            CaseSensitive = caseSensitive;
            Style = style;
            Patterns = patterns;
        }

        public bool CaseSensitive { get; }

        public string Name { get; }
        public IDictionary<string, Pattern> Patterns { get; }
        public Style Style { get; }

        public string GetRegexPattern()
        {
            var allPatterns = new StringBuilder();
            var blockPatterns = new StringBuilder();
            var markupPatterns = new StringBuilder();
            var wordPatterns = new StringBuilder();

            foreach (var pattern in Patterns.Values)
                switch (pattern) {
                    case BlockPattern _:
                        if (blockPatterns.Length > 1)
                            blockPatterns.Append("|");

                        blockPatterns.AppendFormat("(?'{0}'{1})", pattern.Name, pattern.GetRegexPattern());

                        break;

                    case MarkupPattern _:
                        if (markupPatterns.Length > 1)
                            markupPatterns.Append("|");

                        markupPatterns.AppendFormat("(?'{0}'{1})", pattern.Name, pattern.GetRegexPattern());

                        break;

                    case WordPattern _:
                        if (wordPatterns.Length > 1)
                            wordPatterns.Append("|");

                        wordPatterns.AppendFormat("(?'{0}'{1})", pattern.Name, pattern.GetRegexPattern());

                        break;
                }

            if (blockPatterns.Length > 0)
                allPatterns.AppendFormat("(?'blocks'{0})+?", blockPatterns);

            if (markupPatterns.Length > 0)
                allPatterns.AppendFormat("|(?'markup'{0})+?", markupPatterns);

            if (wordPatterns.Length > 0)
                allPatterns.AppendFormat("|(?'words'{0})+?", wordPatterns);

            return allPatterns.ToString();
        }

        public override string ToString()
            => Name;
    }
}