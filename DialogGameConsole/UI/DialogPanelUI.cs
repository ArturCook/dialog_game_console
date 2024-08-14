using DialogGameConsole.UI.Entities;
using DialogGameConsole.UI.Util;
using DialogGameConsole.Util;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.UI;

public class DialogPanelUI : Renderable, IElementUI
{
	static DialogPanelUI()
	{
		_borderSmall = string.Join("", Enumerable.Range(0, _borderSmallSize).Select(_ => " "));
		_borderLarge = string.Join("", Enumerable.Range(0, _borderLargeSize).Select(_ => " "));
	}

	private static readonly int _borderSmallSize = 1;
	private static readonly int _borderLargeSize = 9;

	private static readonly string _borderSmall;
	private static readonly string _borderLarge;

	private readonly int _width;
	private readonly int _dialogBoxWidth;
	private readonly int _dialogTextWidth;

	private readonly int _height;

	private long? _typingTimerNPC;
	private long? _typingTimerPC;

	private readonly List<DialogPanelTextUI> _panelTexts = new();

	private readonly Table _table;

	private bool _needRefresh;
	
	private string bottomLeftBorder = "⎩";
	private string topRightBorder = "⎫";
	private string bottomRightBorder = " ⎭";
	private string topLeftBorder = "⎧ ";
	private string leftBorderSingle = "⸢ ";
	private string rightBorderSingle = " ⸥";
	private string leftBorder = "⋮ ";
	private string rightBorder = " ⋮";

	// Reference Characters:
	// ⎧⎢⎥⎨ ◜⎧╭ ⌜⎧⎭⎫⎩ ⥌⦙⋮⨡

	private Style _leftStyle = Palette.TextPallete.TextStyleRed;
	
	private Style _rightStyle = Palette.TextPallete.TextStyleBlue;

	private Style BkgStyle = Palette.TextPallete.TextStyleWhite;

	public void EnableNpcTyping() => _typingTimerNPC = 0;
	public void DisableNpcTyping() => _typingTimerNPC = null;

	public void EnablePlayerTyping() => _typingTimerPC = 0;
	public void DisablePlayerTyping() => _typingTimerPC = null;

	public void SetNpcStyle(Style style) => _leftStyle = style;
	public void SetPlayerStyle(Style style) => _rightStyle = style;

	public int GetHeight() => _height + 2;

	public void UpdateTime(long dt)
	{
		if(_typingTimerNPC.HasValue)
        {
			var typingStageNPC = AnimationUI.GetTypingStage(_typingTimerNPC);
			_typingTimerNPC += dt;
			_needRefresh = _needRefresh || (AnimationUI.GetTypingStage(_typingTimerNPC) != typingStageNPC);
		}
		if (_typingTimerPC.HasValue)
		{
			var typingStagePC = AnimationUI.GetTypingStage(_typingTimerPC);
			_typingTimerPC += dt;
			_needRefresh = _needRefresh || (AnimationUI.GetTypingStage(_typingTimerPC) != typingStagePC);
		}

	}

	public bool NeedRefresh() => _needRefresh;

	public DialogPanelUI(int width, int height)
    {
        _width = width;
		_dialogBoxWidth = width - 2;
		_dialogTextWidth = _dialogBoxWidth - _borderSmallSize - _borderLargeSize - leftBorder.Length - rightBorder.Length;
		_height = height;

		foreach(var i in Enumerable.Range(0, _height-1))
        {
			_panelTexts.Add(DialogPanelTextUI.Empty(_dialogBoxWidth, BkgStyle, BkgStyle, i));
		}

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

	public void AddNpcDialog(string text) => AddDialog(text, Justify.Left, _leftStyle);

	public void AddPlayerDialog(string text) => AddDialog(text, Justify.Right, _rightStyle);

	public void Clear()
    {
		_panelTexts.Clear();
		foreach (var i in Enumerable.Range(0, _height - 1))
		{
			_panelTexts.Add(DialogPanelTextUI.Empty(_dialogBoxWidth, BkgStyle, BkgStyle, i));
		}
	}

	private void AddDialog(string text, Justify aligment, Style style)
	{
		foreach (var line in text.Split("\n"))
		{
			var words = line.Split(" ")
				.Select(t => t.Trim())
				.Where(t => t.Length > 0)
				.SelectMany(word => word.Chunk(_dialogTextWidth))
				.Select(word => (word.Length, string.Join("", word)));

			var lineLength = 0;
			var lineTexts = new List<string>();
			var lineWords = new List<string>();
			foreach (var (length, word) in words)
			{
				if (lineLength + length > _dialogTextWidth)
				{
					lineTexts.Add(string.Join(" ", lineWords));
					lineLength = 0;
					lineWords.Clear();
				}
				lineWords.Add(word);
				lineLength += length + 1;
			}
			if (lineWords.Any())
				lineTexts.Add(string.Join(" ", lineWords));

			var maxLength = lineTexts.Max(x => x.Length);

			foreach (var (i, lineText) in lineTexts.WithIndex()) {
				var firstLine = i == 0;
				var lastLine = i == lineTexts.Count - 1;
				UpdateDialogLine(lineText, aligment, style, maxLength, firstLine, lastLine);
			}
		}
	}

	private void UpdateDialogLine(string lineText, Justify aligment, Style style, int maxLength, bool firstLine, bool lastLine)
	{
		var singleWord = firstLine && lastLine;
		string leftText, centerText, rightText;

		if (aligment == Justify.Left) {
			var borderLeft = singleWord ? leftBorderSingle : firstLine ? topLeftBorder : leftBorder;
			var borderRight = singleWord ? rightBorderSingle : lastLine ? bottomRightBorder : rightBorder;

			leftText = _borderSmall + borderLeft;
			centerText = lineText.PadRight(maxLength, ' ');
			rightText = borderRight.PadRight(_dialogTextWidth - maxLength + borderLeft.Length, ' ') + _borderLarge;
		}
			
		else {
			var borderLeft = singleWord ? leftBorderSingle : firstLine ? topLeftBorder : leftBorder;
			var borderRight = singleWord ? rightBorderSingle : lastLine ? bottomRightBorder : rightBorder;

			rightText = borderRight + _borderSmall;
			centerText = lineText.PadLeft(maxLength, ' ');
			leftText = _borderLarge + borderLeft.PadLeft(_dialogTextWidth - maxLength + borderLeft.Length, ' ');
		}

		_panelTexts.RemoveAt(0);
		foreach(var panel in _panelTexts) {
			panel.IncrementIndex();
        }
		_panelTexts.Add(new(leftText, centerText, rightText, aligment, style, BkgStyle, _panelTexts.Length()));
	}

	protected override Measurement Measure(RenderContext context, int maxWidth) {
            return new Measurement(_width, _width);
       }

	protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
	{
		_table.Rows.Clear();

		foreach (var (panelText, i) in _panelTexts.Select((x,i)=>(x,i)))
		{
			_table.AddRow(panelText);
		}

		var loadingText1 = AnimationUI.GeTypingText(_typingTimerNPC);
		var loadingText2 = AnimationUI.GeTypingText(_typingTimerPC);

		var paragraph = new Paragraph();
		paragraph.Append(_borderSmall, Palette.TextPallete.TextStyleWhite);
		paragraph.Append(loadingText1.PadRight(_dialogBoxWidth - 2 * _borderSmallSize - AnimationUI.TypingAnimationLength), Palette.TextPallete.TextStyleRed);
		paragraph.Append(loadingText2, Palette.TextPallete.TextStyleBlue);
		paragraph.Append(_borderSmall, Palette.TextPallete.TextStyleWhite);
		_table.AddRow(paragraph);

		return ((IRenderable)_table).Render(context, _width);
	}

}
