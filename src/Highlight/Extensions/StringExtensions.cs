namespace Highlight.Extensions
{
    internal static class StringExtensions
    {
        public static float ToSingle(this string input, float defaultValue)
            => float.TryParse(input, out var result)
                ? result
                : defaultValue;
    }
}