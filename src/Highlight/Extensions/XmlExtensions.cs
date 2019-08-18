namespace Highlight.Extensions
{
    using System;
    using System.Xml.Linq;

    internal static class XmlExtensions
    {
        public static string GetAttributeValue(this XElement element, XName name)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var attribute = element.Attribute(name);

            return attribute?.Value;
        }
    }
}