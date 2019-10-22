namespace Highlight.Engines
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Highlight.Patterns;

    public abstract class EngineGeneric<T> : IEngineGeneric<T>
    {

        public T Highlight(Definition definition, string input)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            var output = PreHighlight(definition, input);
            T output2 = HighlightUsingRegex(definition, output);
            output2 = PostHighlight(definition, output2);

            return output2;
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

        protected abstract T HighlightUsingRegex(Definition definition, string input);
    }
}