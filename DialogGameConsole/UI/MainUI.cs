using DialogGameConsole.UI.Util;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.UI;

public class MainUI : Renderable, IElementUI
{
    private const int _borderSize = 1;

    private const int _width = ConstantsUI.WindowX;

    private readonly Table _table;

    private readonly List<IElementUI> _uiElements = new();

    public int GetInternalWidth() => GetWidth() - 2 * _borderSize;

    public int GetWidth() => _width;

    public int GetHeight() => ConstantsUI.WindowY; //_uiElements.Sum(e=>e.GetHeight()) + 2 * _borderSize;

    public void AddElement(IElementUI element)
    {
        _uiElements.Add(element);
    }

    public MainUI()
    {
        _table = new Table()
            .HideHeaders()
            .Centered()
            .BorderStyle(Palette.BorderStyle)
            .Border(TableBorder.HeavyEdge)
            .Width(_width)
            .Expand()
            .AddColumn(new TableColumn("Main").Padding(0, 0));
    }

    public void UpdateTime(long dt) => _uiElements.ForEach(e => e.UpdateTime(dt));

    public bool NeedRefresh() => _uiElements.Any(e => e.NeedRefresh());

    protected override Measurement Measure(RenderContext context, int maxWidth)
    {
        return new Measurement(_width, _width);
    }

    protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
    {
        _table.Rows.Clear();
        var height = 0;
        foreach (var element in _uiElements)
        {
            height += element.GetHeight();
            if (element.GetHeight() > 0)
                _table.AddRow(element);
        }
        foreach(var i in Enumerable.Range(0, GetHeight() - height - 2))
            _table.AddRow(new Paragraph("".PadLeft(_width-2), Palette.BackgroundFull));

        return ((IRenderable)_table).Render(context, _width);
    }
}
