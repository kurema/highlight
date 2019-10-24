namespace Highlight
{
    using Highlight.Configuration;
    using Highlight.Engines;


    public class Highlighter : HighlighterGeneric<string>
    {
        public Highlighter(IEngineGeneric<string> engine) : base(engine)
        {
        }

        public Highlighter(IEngineGeneric<string> engine, IConfiguration configuration) : base(engine, configuration)
        {
        }
    }
}