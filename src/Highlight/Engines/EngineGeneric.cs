namespace Highlight.Engines
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Highlight.Patterns;

    public abstract class EngineGeneric<TResult,TText> : IEngineGeneric<TResult>
    {
        protected const RegexOptions DEFAULT_REGEX_OPTIONS = RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace;

        public TResult Highlight(Definition definition, string input)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            var output = PreHighlight(definition, input);
            TResult outputT = HighlightUsingRegex(definition, output);
            outputT = PostHighlight(definition, outputT);

            return outputT;
        }

        protected virtual TResult PostHighlight(Definition definition, TResult input)
            => input;

        protected virtual string PreHighlight(Definition definition, string input)
            => input;

        protected abstract TText ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match);
        protected abstract TText ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match);
        protected abstract TText ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match);

        protected abstract TText ProcessSimpleConvert(string input);

        protected TText ElementMatchHandler(Definition definition, Match match)
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


        protected abstract TResult HighlightUsingRegex(Definition definition, string input);

        protected (string, TText)[] SplitUsingRegex(Definition definition, string input)
        {
            var regexOptions = GetRegexOptions(definition);
            var regexPattern = definition.GetRegexPattern();

            var result = new List<(string, TText)>();
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
                result.Add((preText, default(TText)));
            }
            return result.ToArray();
        }

        protected TResult CombineUsingRegex(Definition definition, string input,Func<TText[],TResult> combiner)
        {
            var splited = SplitUsingRegex(definition, input);
            var list = new List<TText>();
            foreach(var item in splited)
            {
                if (item.Item1 != null) list.Add(ProcessSimpleConvert(item.Item1));
                if (item.Item2 != null) list.Add(item.Item2);
            }
            return combiner(list.ToArray());
        }
    }
}