namespace Highlight.Engines
{
    using System.Text.RegularExpressions;
    using Highlight.Patterns;

    public abstract class Engine : EngineGeneric<string>
    {
        protected override string ProcessSimpleConvert(string input) => input;
        private const RegexOptions DEFAULT_REGEX_OPTIONS = RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace;

        private MatchEvaluator GetMatchEvaluator(Definition definition)
        {
            return match => ElementMatchHandler(definition, match);
        }

        private RegexOptions GetRegexOptions(Definition definition)
            => definition.CaseSensitive
                ? DEFAULT_REGEX_OPTIONS | RegexOptions.IgnoreCase
                : DEFAULT_REGEX_OPTIONS;

        protected override string HighlightUsingRegex(Definition definition, string input)
        {
            var regexOptions = GetRegexOptions(definition);
            var evaluator = GetMatchEvaluator(definition);
            var regexPattern = definition.GetRegexPattern();

            return Regex.Replace(input, regexPattern, evaluator, regexOptions);
        }
    }
}