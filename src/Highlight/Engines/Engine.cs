namespace Highlight.Engines
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Highlight.Patterns;

    public abstract class Engine : IEngine
    {
        private const RegexOptions DEFAULT_REGEX_OPTIONS = RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace;

        public string Highlight(Definition definition, string input)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            var output = PreHighlight(definition, input);
            output = HighlightUsingRegex(definition, output);
            output = PostHighlight(definition, output);

            return output;
        }

        protected virtual string PostHighlight(Definition definition, string input)
            => input;

        protected virtual string PreHighlight(Definition definition, string input)
            => input;

        protected abstract string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match);
        protected abstract string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match);
        protected abstract string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match);

        private string ElementMatchHandler(Definition definition, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var pattern = definition.Patterns.First
                                     (
                                         x => match.Groups[x.Key]
                                                   .Success
                                     )
                                    .Value;

            return pattern switch
            {
                BlockPattern blockPattern => ProcessBlockPatternMatch(definition, blockPattern, match),
                MarkupPattern markupPattern => ProcessMarkupPatternMatch(definition, markupPattern, match),
                WordPattern wordPattern => ProcessWordPatternMatch(definition, wordPattern, match),
                _ => match.Value,
            };
        }

        private MatchEvaluator GetMatchEvaluator(Definition definition)
        {
            return match => ElementMatchHandler(definition, match);
        }

        private RegexOptions GetRegexOptions(Definition definition)
            => definition.CaseSensitive
                ? DEFAULT_REGEX_OPTIONS | RegexOptions.IgnoreCase
                : DEFAULT_REGEX_OPTIONS;

        private string HighlightUsingRegex(Definition definition, string input)
        {
            var regexOptions = GetRegexOptions(definition);
            var evaluator = GetMatchEvaluator(definition);
            var regexPattern = definition.GetRegexPattern();

            return Regex.Replace(input, regexPattern, evaluator, regexOptions);
        }
    }
}