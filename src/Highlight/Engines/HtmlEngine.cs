namespace Highlight.Engines
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Highlight.Patterns;

    public class HtmlEngine : Engine
    {
        private const string CLASS_SPAN_FORMAT = "<span class=\"{0}\">{1}</span>";
        private const string STYLE_SPAN_FORMAT = "<span style=\"{0}\">{1}</span>";

        public bool UseCss { get; set; }

        protected override string PostHighlight(Definition definition, string input)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (!UseCss)
            {
                var cssStyle = HtmlEngineHelper.CreatePatternStyle(definition.Style);

                return string.Format(STYLE_SPAN_FORMAT, cssStyle, input);
            }
            else
            {
                var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, null);

                return string.Format(CLASS_SPAN_FORMAT, cssClassName, input);
            }
        }

        protected override string PreHighlight(Definition definition, string input)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            return HttpUtility.HtmlEncode(input);
        }

        protected override string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (!UseCss)
            {
                var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

                return string.Format(STYLE_SPAN_FORMAT, patternStyle, match.Value);
            }
            else
            {
                var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name);

                return string.Format(CLASS_SPAN_FORMAT, cssClassName, match.Value);
            }
        }

        protected override string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var result = new StringBuilder();

            if (!UseCss)
            {
                var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.BracketColors, pattern.Style.Font);

                result.AppendFormat
                (
                    STYLE_SPAN_FORMAT,
                    patternStyle,
                    match.Groups["openTag"]
                         .Value
                );

                result.Append
                (
                    match.Groups["ws1"]
                         .Value
                );

                patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

                result.AppendFormat
                (
                    STYLE_SPAN_FORMAT,
                    patternStyle,
                    match.Groups["tagName"]
                         .Value
                );

                if (pattern.HighlightAttributes)
                {
                    var highlightedAttributes = ProcessMarkupPatternAttributeMatches(definition, pattern, match);
                    result.Append(highlightedAttributes);
                }

                result.Append
                (
                    match.Groups["ws5"]
                         .Value
                );

                patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.BracketColors, pattern.Style.Font);

                result.AppendFormat
                (
                    STYLE_SPAN_FORMAT,
                    patternStyle,
                    match.Groups["closeTag"]
                         .Value
                );
            }
            else
            {
                var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "Bracket");

                result.AppendFormat
                (
                    CLASS_SPAN_FORMAT,
                    cssClassName,
                    match.Groups["openTag"]
                         .Value
                );

                result.Append
                (
                    match.Groups["ws1"]
                         .Value
                );

                cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "TagName");

                result.AppendFormat
                (
                    CLASS_SPAN_FORMAT,
                    cssClassName,
                    match.Groups["tagName"]
                         .Value
                );

                if (pattern.HighlightAttributes)
                {
                    var highlightedAttributes = ProcessMarkupPatternAttributeMatches(definition, pattern, match);
                    result.Append(highlightedAttributes);
                }

                result.Append
                (
                    match.Groups["ws5"]
                         .Value
                );

                cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "Bracket");

                result.AppendFormat
                (
                    CLASS_SPAN_FORMAT,
                    cssClassName,
                    match.Groups["closeTag"]
                         .Value
                );
            }

            return result.ToString();
        }

        protected override string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
        {
            if (!UseCss)
            {
                var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

                return string.Format(STYLE_SPAN_FORMAT, patternStyle, match.Value);
            }
            else
            {
                var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name);

                return string.Format(CLASS_SPAN_FORMAT, cssClassName, match.Value);
            }
        }

        private string ProcessMarkupPatternAttributeMatches(Definition definition, MarkupPattern pattern, Match match)
        {
            var result = new StringBuilder();

            for (var i = 0;
                i <
                match.Groups["attribute"]
                     .Captures.Count;
                i++)
            {
                result.Append
                (
                    match.Groups["ws2"]
                         .Captures[i]
                         .Value
                );

                if (!UseCss)
                {
                    var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.AttributeNameColors, pattern.Style.Font);

                    result.AppendFormat
                    (
                        STYLE_SPAN_FORMAT,
                        patternStyle,
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

                    patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.AttributeValueColors, pattern.Style.Font);

                    result.AppendFormat
                    (
                        STYLE_SPAN_FORMAT,
                        patternStyle,
                        match.Groups["attribValue"]
                             .Captures[i]
                             .Value
                    );
                }
                else
                {
                    var cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "AttributeName");

                    result.AppendFormat
                    (
                        CLASS_SPAN_FORMAT,
                        cssClassName,
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

                    cssClassName = HtmlEngineHelper.CreateCssClassName(definition.Name, pattern.Name + "AttributeValue");

                    result.AppendFormat
                    (
                        CLASS_SPAN_FORMAT,
                        cssClassName,
                        match.Groups["attribValue"]
                             .Captures[i]
                             .Value
                    );
                }
            }

            return result.ToString();
        }
    }
}