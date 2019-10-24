namespace Highlight.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Highlight.Extensions;
    using Highlight.Patterns;

    public class XmlConfiguration : IConfiguration
    {
        private IDictionary<string, Definition> _definitions;

        public XmlConfiguration(XDocument xmlDocument)
            => XmlDocument = xmlDocument ?? throw new ArgumentNullException(nameof(xmlDocument));

        protected XmlConfiguration() { }

        public IDictionary<string, Definition> Definitions
            => GetDefinitions();

        protected XDocument XmlDocument { get; set; }
        //Deserialization may be better. But lots of fix is needed.
        //protected Highlight.Definitions.definitions DefinitionsXml { get; set; }

        private BlockPattern GetBlockPattern(XElement patternElement)
        {
            var name = patternElement.GetAttributeValue("name");
            var style = GetPatternStyle(patternElement);
            var beginsWith = patternElement.GetAttributeValue("beginsWith");
            var endsWith = patternElement.GetAttributeValue("endsWith");
            var escapesWith = patternElement.GetAttributeValue("escapesWith");

            return new BlockPattern(name, style, beginsWith, endsWith, escapesWith);
        }

        private Definition GetDefinition(XElement definitionElement)
        {
            var name = definitionElement.GetAttributeValue("name");
            IDictionary<string, Pattern> patterns = GetPatterns(definitionElement);
            var caseSensitive = bool.Parse(definitionElement.GetAttributeValue("caseSensitive"));
            var style = GetDefinitionStyle(definitionElement);

            return new Definition(name, caseSensitive, style, patterns);
        }

        private ColorPair GetDefinitionColors(XElement fontElement)
        {
            var foreColor = Color.FromName(fontElement.GetAttributeValue("foreColor"));
            var backColor = Color.FromName(fontElement.GetAttributeValue("backColor"));

            return new ColorPair(foreColor, backColor);
        }

        private Font GetDefinitionFont(XElement fontElement)
        {
            var fontName = fontElement.GetAttributeValue("name");
            var fontSize = Convert.ToSingle(fontElement.GetAttributeValue("size"));
            var fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), fontElement.GetAttributeValue("style"), true);

            return new Font(fontName, fontSize, fontStyle);
        }

        private IDictionary<string, Definition> GetDefinitions()
            => _definitions ??= XmlDocument
                               .Descendants("definition")
                               .Select(GetDefinition)
                               .ToDictionary(x => x.Name);

        private Style GetDefinitionStyle(XNode definitionElement)
        {
            const string XPATH = "default/font";
            var fontElement = definitionElement.XPathSelectElement(XPATH);
            var colors = GetDefinitionColors(fontElement);
            var font = GetDefinitionFont(fontElement);

            return new Style(colors, font);
        }

        private MarkupPattern GetMarkupPattern(XElement patternElement)
        {
            var name = patternElement.GetAttributeValue("name");
            var style = GetPatternStyle(patternElement);
            var highlightAttributes = bool.Parse(patternElement.GetAttributeValue("highlightAttributes"));
            var bracketColors = GetMarkupPatternBracketColors(patternElement);
            var attributeNameColors = GetMarkupPatternAttributeNameColors(patternElement);
            var attributeValueColors = GetMarkupPatternAttributeValueColors(patternElement);

            return new MarkupPattern(name, style, highlightAttributes, bracketColors, attributeNameColors, attributeValueColors);
        }

        private ColorPair GetMarkupPatternAttributeNameColors(XContainer patternElement)
        {
            const string DESCENDANT_NAME = "attributeNameStyle";

            return GetMarkupPatternColors(patternElement, DESCENDANT_NAME);
        }

        private ColorPair GetMarkupPatternAttributeValueColors(XContainer patternElement)
        {
            const string DESCENDANT_NAME = "attributeValueStyle";

            return GetMarkupPatternColors(patternElement, DESCENDANT_NAME);
        }

        private ColorPair GetMarkupPatternBracketColors(XContainer patternElement)
        {
            const string DESCENDANT_NAME = "bracketStyle";

            return GetMarkupPatternColors(patternElement, DESCENDANT_NAME);
        }

        private ColorPair GetMarkupPatternColors(XContainer patternElement, XName descendantName)
        {
            var fontElement = patternElement.Descendants("font")
                                            .Single();

            var element = fontElement.Descendants(descendantName)
                                     .SingleOrDefault();

            if (element != null)
            {
                var colors = GetPatternColors(element);

                return colors;
            }

            return null;
        }

        private Pattern GetPattern(XElement patternElement)
        {
            const StringComparison STRING_COMPARISON = StringComparison.OrdinalIgnoreCase;
            var patternType = patternElement.GetAttributeValue("type");

            if (patternType.Equals("block", STRING_COMPARISON))
                return GetBlockPattern(patternElement);

            if (patternType.Equals("markup", STRING_COMPARISON))
                return GetMarkupPattern(patternElement);

            if (patternType.Equals("word", STRING_COMPARISON))
                return GetWordPattern(patternElement);

            throw new InvalidOperationException($"Unknown pattern type: {patternType}");
        }

        private ColorPair GetPatternColors(XElement fontElement)
        {
            var foreColor = Color.FromName(fontElement.GetAttributeValue("foreColor"));
            var backColor = Color.FromName(fontElement.GetAttributeValue("backColor"));

            return new ColorPair(foreColor, backColor);
        }

        private Font GetPatternFont(XElement fontElement, Font defaultFont = null)
        {
            var fontFamily = fontElement.GetAttributeValue("name");

            if (fontFamily == null)
                return defaultFont;

            var emSize = fontElement.GetAttributeValue("size")
                                    .ToSingle(11f);

            var style = fontElement.GetAttributeValue("style").Parse(FontStyle.Regular, true);

            return new Font(fontFamily, emSize, style);

        }

        private IDictionary<string, Pattern> GetPatterns(XContainer definitionElement)
        {
            Dictionary<string, Pattern> patterns = definitionElement
                                                  .Descendants("pattern")
                                                  .Select(GetPattern)
                                                  .ToDictionary(x => x.Name);

            return patterns;
        }

        private Style GetPatternStyle(XContainer patternElement)
        {
            var fontElement = patternElement.Descendants("font")
                                            .Single();

            var colors = GetPatternColors(fontElement);
            var font = GetPatternFont(fontElement);

            return new Style(colors, font);
        }

        private IEnumerable<string> GetPatternWords(XContainer patternElement)
        {
            var words = new List<string>();
            IEnumerable<XElement> wordElements = patternElement.Descendants("word");

            words.AddRange
            (
                from wordElement in wordElements
                select Regex.Escape(wordElement.Value)
            );

            return words;
        }

        private WordPattern GetWordPattern(XElement patternElement)
        {
            var name = patternElement.GetAttributeValue("name");
            var style = GetPatternStyle(patternElement);
            IEnumerable<string> words = GetPatternWords(patternElement);

            return new WordPattern(name, style, words);
        }
    }
}