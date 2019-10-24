namespace Highlight
{
    using System;
    using Highlight.Configuration;
    using Highlight.Engines;

    public class HighlighterGeneric<T>
    {
        public HighlighterGeneric(IEngineGeneric<T> engine, IConfiguration configuration)
        {
            Engine = engine;
            Configuration = configuration;
        }

        public HighlighterGeneric(IEngineGeneric<T> engine)
        : this(engine, new DefaultConfiguration()) { }

        public IConfiguration Configuration { get; set; }
        public IEngineGeneric<T> Engine { get; set; }

        public T Highlight(string definitionName, string input)
        {
            if (definitionName == null)
                throw new ArgumentNullException(nameof(definitionName));

            if (!Configuration.Definitions.ContainsKey(definitionName))
            {
                //I want to write like this.
                //if (typeof(T) == typeof(string)) return (T)input;
                //But it's wrong code. So instead...
                if (input is T result) return result;

                return default(T);
            }

            var definition = Configuration.Definitions[definitionName];

            return Engine.Highlight(definition, input);
        }
    }
}