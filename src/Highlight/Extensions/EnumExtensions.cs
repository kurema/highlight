namespace Highlight.Extensions
{
    using System;

    internal static class EnumExtensions
    {
        public static T Parse<T>(this string value, T defaultValue) where T : struct
            => Parse(value, defaultValue, false);

        public static T Parse<T>(this string value, T defaultValue, bool ignoreCase) where T : struct
            => Enum.TryParse(value, ignoreCase, out T result)
            ? result
            : defaultValue;
    }
}