using DialogGameConsole.Options.Enums;
using DialogGameConsole.UI.Entities;
using DialogGameConsole.UI.Util;
using DialogGameConsole.Util;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.UI;

public sealed class OptionsUI : Renderable, IElementUI
{
	private const int _maxOptions = 3;

	private readonly int _width;

	private readonly int _internalWidth;

	private const int _height = 5;

	private readonly int _textLength;

	private readonly ConcurrentDictionary<int, DialogOptionUI> _options = new();

	private readonly Table _table;

	private int _selectedOptionIndex = 0;

	public DialogOptionUI SelectedOption => _options[_selectedOptionIndex];

	private readonly Style _selectedStyle = Palette.TextPallete.TextStyleBlue;

	private readonly Style _defaultStyle = Palette.TextPallete.TextStyleWhite;

	private readonly Style _disabledStyle = Palette.TextPallete.TextStyleGray;

	private readonly static int _selectedTextLength = 3;

	private readonly static string _selectedText = " " + ConstantsUI.SelectionMarker + " ";

	private readonly static string _nonSelectedText = "   ";

	private bool _needRefresh;

	private readonly string _borderTop;

	private readonly string _borderBottom;

	private readonly string _borderLeft;

	private readonly string _borderRight;

	private long?[] _optionTimer = new long?[_maxOptions];

	private bool _hasLoading = false;

	public int GetHeight() => _height;

	public OptionsUI(int width)
	{
		_width = width;
		_internalWidth = width - 2;
		_textLength = _internalWidth - _selectedTextLength;
		foreach (var i in Enumerable.Range(0, _maxOptions))
		{
			_options[i] = DialogOptionUI.New();
		}

		var border = new SquareBoxBorder();
		_borderTop = border.GetPart(BoxBorderPart.TopLeft) +
			string.Join("", Enumerable.Range(0, _internalWidth).Select(_ => border.GetPart(BoxBorderPart.Top))) +
			border.GetPart(BoxBorderPart.TopRight);
		_borderBottom = border.GetPart(BoxBorderPart.BottomLeft) +
			string.Join("", Enumerable.Range(0, _internalWidth).Select(_ => border.GetPart(BoxBorderPart.Top))) +
			border.GetPart(BoxBorderPart.BottomRight);
		_borderLeft = border.GetPart(BoxBorderPart.Left);
		_borderRight = border.GetPart(BoxBorderPart.Right);

		_table = new Table()
			.HideHeaders()
			.Centered()
			.Border(TableBorder.HeavyHead)
			.BorderStyle(Palette.InternalBorderStyle)
			.Width(_width)
			.AddColumn(new TableColumn("Main")
			{
				Padding = new Padding(0),
				Width = _width
			});
	}

	public void RemoveOption(int index)
	{
		SetOption(index, "", OptionStatus.Empty);
	}

	public void SetOption(int index, string text, OptionStatus status)
    {
		if (index < 0 || index >= _maxOptions) 
			throw new ArgumentException($"Index is invalid: {index}");

		_options[index].SetStatus(status);
		if(status != OptionStatus.Empty)
			_options[index].SetText(text);

		_hasLoading = _optionTimer.Any(t => t.HasValue);
		_needRefresh = true;
	}

	public void EnableOptionTyping(int index) => _optionTimer[index] = 0;
	public void DisableOptionTyping(int index) => _optionTimer[index] = null;

	public void SetSelected(int index)
	{
		_selectedOptionIndex = index;
		_needRefresh = true;
	}

	public void UpdateTime(long dt)
	{
		if (!_hasLoading) return;

		int optionStage;
        for (int i = 0; i < _maxOptions; i++) {
			if (!_optionTimer[i].HasValue) continue;
			
			optionStage = AnimationUI.GetTypingStage(_optionTimer[i]);
			_optionTimer[i] += dt;
			_needRefresh = _needRefresh || (AnimationUI.GetTypingStage(_optionTimer[i]) != optionStage);
		}
	}

	public bool NeedRefresh() => _needRefresh;

	protected override Measurement Measure(RenderContext context, int maxWidth)
    {
		return new Measurement(_width, _width);
    }

	protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
	{
		_table.Rows.Clear();

		yield return new Segment(_borderTop, Palette.InternalBorderStyle);
		yield return Segment.LineBreak;
		foreach (var (i, option) in _options.ToList())
		{
			yield return new Segment(_borderLeft, Palette.InternalBorderStyle);

			var isSelected = (i == _selectedOptionIndex);
			var isDisabled = !option.IsSelectable();
			var style = isDisabled ? _disabledStyle : isSelected ? _selectedStyle : _defaultStyle;

			var selectionText = isSelected ? _selectedText : _nonSelectedText;

			yield return new Segment(selectionText, style);

			if (option.IsEmpty())
				yield return new Segment("".PadRight(_textLength, ' '), Palette.BackgroundFull);
			else if (option.IsLoading())
			{
				var text = AnimationUI.GeTypingText(_optionTimer[i], i).PadRight(_textLength, ' ');
				yield return new Segment(text, Palette.FirePalette.BkgStyleColor11);
			}
			else
				yield return new Segment(option.Text.PadRight(_textLength, ' '), style);

			yield return new Segment(_borderRight, Palette.InternalBorderStyle);
			yield return Segment.LineBreak;
		}

		yield return new Segment(_borderBottom, Palette.InternalBorderStyle);
	}
}
