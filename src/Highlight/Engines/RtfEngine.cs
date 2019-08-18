namespace Highlight.Engines
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Highlight.Patterns;

    // TODO: Clean up and refactor big methods into smaller, more manageable chunks.
    public class RtfEngine : Engine
    {
        private const string RTF_FORMAT = "{0} {1}";
        private readonly ArrayList _colors = new ArrayList();
        private readonly ArrayList _fonts = new ArrayList();

        protected override string PostHighlight(Definition definition, string input)
        {
            var result = input
                        .Replace("{", @"\{")
                        .Replace("}", @"\}")
                        .Replace("\t", @"\tab ")
                        .Replace("\r\n", @"\par ");

            var fontList = BuildFontList();
            var colorList = BuildColorList();

            return $@"{{\rtf1\ansi{{\fonttbl{{{fontList}}}}}{{\colortbl;{colorList}}}{result}}}";
        }

        protected override string PreHighlight(Definition definition, string input)
            => HttpUtility.HtmlEncode(input);

        protected override string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
        {
            var style = CreateRtfPatternStyle(pattern.Style.Colors.ForeColor, pattern.Style.Colors.BackColor, pattern.Style.Font);

            return "{" + string.Format(RTF_FORMAT, style, match.Value) + "}";
        }

        protected override string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
        {
            var builder = new StringBuilder();
            var style = CreateRtfPatternStyle(pattern.Style.Colors.ForeColor, pattern.Style.Colors.BackColor, pattern.Style.Font);
            var bracketStyle = CreateRtfPatternStyle(pattern.BracketColors.ForeColor, pattern.BracketColors.BackColor, pattern.Style.Font);
            string attributeNameStyle = null;
            string attributeValueStyle = null;

            if (pattern.HighlightAttributes)
            {
                attributeNameStyle = CreateRtfPatternStyle
                (pattern.AttributeNameColors.ForeColor, pattern.AttributeNameColors.BackColor, pattern.Style.Font);

                attributeValueStyle = CreateRtfPatternStyle
                (pattern.AttributeValueColors.ForeColor, pattern.AttributeValueColors.BackColor, pattern.Style.Font);
            }

            builder.AppendFormat
            (
                RTF_FORMAT,
                bracketStyle,
                match.Groups["openTag"]
                     .Value
            );

            builder.Append
            (
                match.Groups["ws1"]
                     .Value
            );

            builder.AppendFormat
            (
                RTF_FORMAT,
                style,
                match.Groups["tagName"]
                     .Value
            );

            if (attributeNameStyle != null)
                for (var i = 0;
                    i <
                    match.Groups["attribute"]
                         .Captures.Count;
                    i++)
                {
                    builder.Append
                    (
                        match.Groups["ws2"]
                             .Captures[i]
                             .Value
                    );

                    builder.AppendFormat
                    (
                        RTF_FORMAT,
                        attributeNameStyle,
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

                    builder.AppendFormat
                    (
                        RTF_FORMAT,
                        attributeValueStyle,
                        match.Groups["attribValue"]
                             .Captures[i]
                             .Value
                    );
                }

            builder.Append
            (
                match.Groups["ws5"]
                     .Value
            );

            builder.AppendFormat
            (
                RTF_FORMAT,
                bracketStyle,
                match.Groups["closeTag"]
                     .Value
            );

            return "{" + builder + "}";
        }

        protected override string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
        {
            var style = CreateRtfPatternStyle(pattern.Style.Colors.ForeColor, pattern.Style.Colors.BackColor, pattern.Style.Font);

            return "{" + string.Format(RTF_FORMAT, style, match.Value) + "}";
        }

        private string BuildColorList()
        {
            var builder = new StringBuilder();

            foreach (var hexColor in _colors.Cast<HexColor>())
                builder.AppendFormat(@"\red{0}\green{1}\blue{2};", hexColor.Red, hexColor.Green, hexColor.Blue);

            return builder.ToString();
        }

        private string BuildFontList()
        {
            var builder = new StringBuilder();

            for (var i = 0; i < _fonts.Count; i++)
                builder.AppendFormat(@"\f{0} {1};", i, _fonts[i]);

            return builder.ToString();
        }

        private string CreateRtfPatternStyle(Color foreColor, Color backColor, Font font)
            => string.Concat
            (
                @"\cf",
                GetIndexOfColor(foreColor),
                @"\highlight",
                GetIndexOfColor(backColor),
                @"\f",
                GetIndexOfFont(font.Name),
                @"\fs",
                font.Size * 2f
            );

        private int GetIndexOfColor(Color color)
        {
            var color2 = new HexColor();

            if (color.Name.IndexOf("#", StringComparison.Ordinal) > -1)
            {
                color2.Red = int.Parse(color.Name.Substring(1, 2), NumberStyles.AllowHexSpecifier);
                color2.Green = int.Parse(color.Name.Substring(3, 2), NumberStyles.AllowHexSpecifier);
                color2.Blue = int.Parse(color.Name.Substring(5, 2), NumberStyles.AllowHexSpecifier);
            }
            else
            {
                color2.Red = color.R;
                color2.Green = color.G;
                color2.Blue = color.B;
            }

            var index = _colors.IndexOf(color2);

            if (index > -1)
                return index + 1;

            _colors.Add(color2);

            return _colors.Count;
        }

        private int GetIndexOfFont(string font)
        {
            var index = _fonts.IndexOf(font);

            if (index > -1)
                return index + 1;

            _fonts.Add(font);

            return _fonts.Count;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HexColor
        {
            public int Red;
            public int Green;
            public int Blue;
        }
    }
}