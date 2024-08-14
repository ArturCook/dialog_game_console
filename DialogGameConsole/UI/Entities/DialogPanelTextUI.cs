using DialogGameConsole.UI.Util;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Collections.Generic;

namespace DialogGameConsole.UI.Entities;

public class DialogPanelTextUI : Renderable
{
    public int Width { get; }
    public string LeftText { get; }
    public string CenterText { get; }
    public string RightText { get; }
    public Justify Aligment { get; }
    public Style BaseStyle { get; }
    public Style BkgStyle { get; }
    public int Index { get; private set; }

    private static float _intensityFactor = 1.0F;

    private static List<(float, Color)> _blendColors = new() 
    {
        (0.1F * _intensityFactor, Palette.FirePalette.Color3),
        (0.2F * _intensityFactor, Palette.FirePalette.Color3),
        (0.3F * _intensityFactor, Palette.FirePalette.Color3),
        (0.5F * _intensityFactor, Palette.FirePalette.Color3),
        (0.7F * _intensityFactor, Palette.FirePalette.Color3),
        (0.8F * _intensityFactor, Palette.FirePalette.Color4),
        (1.0F * _intensityFactor, Palette.FirePalette.Color4),
        (0.9F * _intensityFactor, Palette.FirePalette.Color4),
        (0.9F * _intensityFactor, Palette.FirePalette.Color5),
        (0.9F * _intensityFactor, Palette.FirePalette.Color7),
        (0.9F * _intensityFactor, Palette.FirePalette.Color7),
        (0.9F * _intensityFactor, Palette.FirePalette.Color8),
        (0.9F * _intensityFactor, Palette.FirePalette.Color8),
    };

    public void IncrementIndex() {
        Index++;
    }

    public DialogPanelTextUI(string leftText, string centerText, string rightText, Justify aligment, 
        Style style, Style bkgStyle, int index) {
        Width = leftText.Length + centerText.Length + rightText.Length;
        LeftText = leftText;
        CenterText = centerText;
        RightText = rightText;
        Aligment = aligment;
        BaseStyle = style;
        BkgStyle = bkgStyle;
        Index = index;
    }

    public static DialogPanelTextUI Empty(int width, Style style, Style bkgStyle, int index) {
        return new DialogPanelTextUI("", "".PadRight(width, ' '), "", Justify.Left, style, bkgStyle, index);
    }

    protected override Measurement Measure(RenderContext context, int maxWidth) {
        return new Measurement(Width, Width);
    }

    protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth) {
        var textColor = BaseStyle.Foreground;
        var style = BaseStyle;
        var style2 = new Style(textColor.Blend(BkgStyle.Background, 0.5F), BkgStyle.Background);

        if (Index < _blendColors.Length()) {
            var (intensity, colorToBlend) = _blendColors[Index];
   
            var blendedColor = textColor.Blend(colorToBlend, intensity);
            style = new Style(blendedColor, BkgStyle.Background);
        }

        yield return new Segment(LeftText, style2);
        yield return new Segment(CenterText, style);
        yield return new Segment(RightText, style2);
    }
}