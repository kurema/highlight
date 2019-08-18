namespace Highlight
{
    using System;
    using Highlight.Configuration;
    using Highlight.Engines;

    public class Highlighter
    {
        public Highlighter(IEngine engine, IConfiguration configuration)
        {
            Engine = engine;
            Configuration = configuration;
        }

        public Highlighter(IEngine engine)
        : this(engine, new DefaultConfiguration()) { }

        public IConfiguration Configuration { get; set; }
        public IEngine Engine { get; set; }

        public string Highlight(string definitionName, string input)
        {
            if (definitionName == null)
                throw new ArgumentNullException(nameof(definitionName));

            if (!Configuration.Definitions.ContainsKey(definitionName))
                return input;

            var definition = Configuration.Definitions[definitionName];

            return Engine.Highlight(definition, input);
        }
    }
}