namespace Highlight.Engines
{
    using System.Drawing;
    using System.Text;
    using Highlight.Patterns;

    internal static class HtmlEngineHelper
    {
        public static string CreateCssClassName(string definition, string pattern)
        {
            var cssClassName = definition
                              .Replace("#", "sharp")
                              .Replace("+", "plus")
                              .Replace(".", "dot")
                              .Replace("-", "");

            return string.Concat(cssClassName, pattern);
        }

        public static string CreatePatternStyle(Style style)
            => CreatePatternStyle(style.Colors, style.Font);

        public static string CreatePatternStyle(ColorPair colors, Font font)
        {
            var patternStyle = new StringBuilder();

            if (colors != null)
            {
                if (colors.ForeColor != Color.Empty)
                    patternStyle.Append("color: ").Append(colors.ForeColor.Name).Append(';');

                if (colors.BackColor != Color.Empty)
                    patternStyle.Append("background-color: ").Append(colors.BackColor.Name).Append(';');
            }

            if (font?.Name != null)
                patternStyle.Append("font-family: ").Append(font.Name).Append(';');

            if (font?.Size > 0f)
                patternStyle.Append("font-size: ").Append(font.Size).Append("px;");

            switch (font?.Style)
            {
                case FontStyle.Regular:
                    patternStyle.Append("font-weight: normal;");

                    break;
                case FontStyle.Bold:
                    patternStyle.Append("font-weight: bold;");

                    break;
            }

            return patternStyle.ToString();
        }
    }
}