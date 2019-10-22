using Highlight.Patterns;

namespace Highlight.Engines
{
    public interface IEngineGeneric<T>
    {
        T Highlight(Definition definition, string input);
    }
}