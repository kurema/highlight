namespace Highlight.Engines
{
    using System.Text.RegularExpressions;
    using Highlight.Patterns;

    public abstract class Engine : EngineGeneric<string>
    {
        protected override string ProcessSimpleConvert(string input) => input;

        protected MatchEvaluator GetMatchEvaluator(Definition definition)
        {
            return match => ElementMatchHandler(definition, match);
        }

        protected override string HighlightUsingRegex(Definition definition, string input)
        {
            var regexOptions = GetRegexOptions(definition);
            var evaluator = GetMatchEvaluator(definition);
            var regexPattern = definition.GetRegexPattern();

            return Regex.Replace(input, regexPattern, evaluator, regexOptions);
        }
    }
}