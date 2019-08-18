namespace Highlight.Patterns
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public sealed class WordPattern : Pattern
    {
        public WordPattern(string name, Style style, IEnumerable<string> words)
        : base(name, style)
            => Words = words;

        public IEnumerable<string> Words { get; }

        public override string GetRegexPattern()
        {
            var str = string.Empty;

            if (!Words.Any())
                return str;

            var nonWords = GetNonWords();

            return $@"(?<![\w{nonWords}])(?=[\w{nonWords}])({string.Join("|", Words.ToArray())})(?<=[\w{nonWords}])(?![\w{nonWords}])";
        }

        private string GetNonWords()
        {
            var input = string.Concat(Words.ToArray());
            var list = new List<string>();

            foreach (var match in Regex.Matches(input, @"\W")
                                       .Cast<Match>()
                                       .Where(x => !list.Contains(x.Value)))
                list.Add(match.Value);

            return string.Concat(list.ToArray());
        }
    }
}