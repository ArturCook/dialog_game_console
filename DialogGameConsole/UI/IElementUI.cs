using Spectre.Console.Rendering;

namespace DialogGameConsole.UI;

public interface IElementUI : IRenderable
{
    int GetHeight();
    void UpdateTime(long dt);
    bool NeedRefresh();
}
