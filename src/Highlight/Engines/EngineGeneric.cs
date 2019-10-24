namespace Highlight.Engines
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Highlight.Patterns;

    public abstract class EngineGeneric<T> : IEngineGeneric<T>
    {
        protected const RegexOptions DEFAULT_REGEX_OPTIONS = RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace;

        public T Highlight(Definition definition, string input)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            var output = PreHighlight(definition, input);
            T outputT = HighlightUsingRegex(definition, output);
            outputT = PostHighlight(definition, outputT);

            return outputT;
        }

        protected virtual T PostHighlight(Definition definition, T input)
            => input;

        protected virtual string PreHighlight(Definition definition, string input)
            => input;

        protected abstract T ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match);
        protected abstract T ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match);
        protected abstract T ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match);

        protected abstract T ProcessSimpleConvert(string input);

        protected T ElementMatchHandler(Definition definition, Match match)
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
                _ => ProcessSimpleConvert(match.Value),
            };
        }

        protected static RegexOptions GetRegexOptions(Definition definition)
    => definition.CaseSensitive
        ? DEFAULT_REGEX_OPTIONS | RegexOptions.IgnoreCase
        : DEFAULT_REGEX_OPTIONS;


        protected abstract T HighlightUsingRegex(Definition definition, string input);

        protected (string,T)[] SplitUsingRegex(Definition definition, string input)
        {
            var regexOptions = GetRegexOptions(definition);
            var regexPattern = definition.GetRegexPattern();

            var result = new System.Collections.Generic.List<(string, T)>();
            var matches = Regex.Matches(input, regexPattern, regexOptions);
            int lastIndex = 0;
            foreach (Match item in matches)
            {
                var preText = input.Substring(lastIndex, item.Index - lastIndex);
                result.Add((preText, ElementMatchHandler(definition, item)));
                lastIndex = item.Index + item.Length;
            }
            if (lastIndex < input.Length)
            {
                var preText = input.Substring(lastIndex, input.Length - lastIndex);
                result.Add((preText, default(T)));
            }
            return result.ToArray();
        }
    }
}