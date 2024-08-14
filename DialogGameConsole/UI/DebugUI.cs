using DialogGameConsole.UI.Util;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.UI;

public class DebugUI : Renderable, IElementUI
{
    private const int _height = 1 + _messageCount;

    private readonly int _width;

    private readonly int _textWidth;

    private const string _line1Text = "Time {0:F1} Speed {1:F1}x";

    private const int _messageCount = 3;

    private List<string> _debugMessages = new(_messageCount);

    private bool _isEnabled;

    public int GetHeight() => _isEnabled ? _height : 0;

    public DebugUI(int width) {
        _width = width;
        _textWidth = width - 2;
        foreach(var i in Enumerable.Range(0, _messageCount)) {
            _debugMessages.Add("".PadRight(_textWidth, ' '));
        }
    }

    internal void Set(bool isEnabled) {
        _isEnabled = isEnabled;
    }

    public void AddMessage(int number, string message) {
        _debugMessages.RemoveAt(0);
        var messageWithIndex = $"{number.ToString().PadLeft(3, '0')} - {message}"
            .PadRight(_textWidth, ' ')
            .Substring(0, _textWidth);
        _debugMessages.Add(messageWithIndex);
    }

    public void UpdateTime(long dt) { }

    public bool NeedRefresh() => false;

    protected override Measurement Measure(RenderContext context, int maxWidth)
    {
        return new Measurement(_width, _width);
    }

    protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
    {
        if (!_isEnabled) yield break;

        var line1Text = string.Format(_line1Text, 0, 0);//Game.GlobalTime, Game.GameSpeed);

        yield return new Segment(ConstantsUI.BorderChar, Palette.InternalBorderStyle);
        yield return new Segment(line1Text.PadRight(_textWidth, ' '), Palette.TextPallete.TextStyleGray);
        yield return new Segment(ConstantsUI.BorderChar + "\n", Palette.InternalBorderStyle);

        foreach(var message in _debugMessages) {
            yield return new Segment(ConstantsUI.BorderChar, Palette.InternalBorderStyle);
            yield return new Segment(message, Palette.InternalBorderStyle);
            yield return new Segment(ConstantsUI.BorderChar + "\n", Palette.InternalBorderStyle);
        }
    }

}
