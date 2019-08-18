namespace Highlight.Engines
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Highlight.Patterns;

    // TODO: Refactor this engine to build proper XML using XLinq.
    public class XmlEngine : Engine
    {
        private const string ELEMENT_FORMAT = "<{0}>{1}</{0}>";

        protected override string PostHighlight(Definition definition, string input)
            => $"<highlightedInput>{input}</highlightedInput>";

        protected override string PreHighlight(Definition definition, string input)
            => HttpUtility.HtmlEncode(input);

        protected override string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
            => ProcessPatternMatch(pattern, match);

        protected override string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
        {
            var builder = new StringBuilder();

            builder.AppendFormat
            (
                ELEMENT_FORMAT,
                "openTag",
                match.Groups["openTag"]
                     .Value
            );

            builder.AppendFormat
            (
                ELEMENT_FORMAT,
                "whitespace",
                match.Groups["ws1"]
                     .Value
            );

            builder.AppendFormat
            (
                ELEMENT_FORMAT,
                "tagName",
                match.Groups["tagName"]
                     .Value
            );

            var builder2 = new StringBuilder();

            for (var i = 0; i < match.Groups["attribute"].Captures.Count; i++)
            {
                builder2.AppendFormat
                (
                    ELEMENT_FORMAT,
                    "whitespace",
                    match.Groups["ws2"]
                         .Captures[i]
                         .Value
                );

                builder2.AppendFormat
                (
                    ELEMENT_FORMAT,
                    "attribName",
                    match.Groups["attribName"]
                         .Captures[i]
                         .Value
                );

                if (string.IsNullOrWhiteSpace
                (
                    match.Groups["attribValue"]
                         .Captures[i]
                         .Value
                ))
                    continue;

                builder2.AppendFormat
                (
                    ELEMENT_FORMAT,
                    "attribValue",
                    match.Groups["attribValue"]
                         .Captures[i]
                         .Value
                );
            }

            builder.AppendFormat(ELEMENT_FORMAT, "attribute", builder2);

            builder.AppendFormat
            (
                ELEMENT_FORMAT,
                "whitespace",
                match.Groups["ws5"]
                     .Value
            );

            builder.AppendFormat
            (
                ELEMENT_FORMAT,
                "closeTag",
                match.Groups["closeTag"]
                     .Value
            );

            return string.Format(ELEMENT_FORMAT, pattern.Name, builder);
        }

        protected override string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
            => ProcessPatternMatch(pattern, match);

        private string ProcessPatternMatch(Pattern pattern, Match match)
            => string.Format(ELEMENT_FORMAT, pattern.Name, match.Value);
    }
}