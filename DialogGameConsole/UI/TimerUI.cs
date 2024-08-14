using DialogGameConsole.UI.Util;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.UI;

public sealed class TimerUI : Renderable, IElementUI
{
	private readonly int _width;

	private readonly int _internalWidth;

	private const int _height = 1;

	private const string _timerText = "Timer: {0:F1}/{1:F2}";

	private int _totalTime;

	private long _currentTime;

	private int _currentWidthPosition;

    private bool _needRefresh;

	private readonly double[] _intervalTimes;

	private readonly int[] _interval1Length;

	private readonly int[] _interval2Length;

	private readonly int[] _interval3Length;

	public int GetHeight() => _height;

	public void SetSeconds(long currentTime)
	{
		var currentWidthPosition = _currentWidthPosition;
		_currentTime = currentTime;
		_currentWidthPosition = (int)((double)_internalWidth * _currentTime / _totalTime);
		_needRefresh = _needRefresh || (currentWidthPosition != _currentWidthPosition);
	}

	internal void ResetTimer(int totalTime)
	{
		_totalTime = totalTime;
		_currentTime = totalTime;
		_currentWidthPosition = _internalWidth;
		_needRefresh = true;

		var totalIntervalTime = 0.0;
		for (var i = 0; i < _intervalTimes.Length; i++)
        {
			var intervalTime = Game.UIRandom.NextDouble() * 2;
			_intervalTimes[i] = intervalTime;
			totalIntervalTime += intervalTime;
		}

		var scaling = (_internalWidth+1) / totalIntervalTime;
		var currentWidth = 0.0;
		var intervalContinuity = 3;
		for (var i = 0; i < _intervalTimes.Length; i++)
		{
			currentWidth += _intervalTimes[i] * scaling;
			_intervalTimes[i] = ((int)currentWidth);
			
			_interval1Length[i] = !(i != 0 && i % intervalContinuity != 0) ? Game.UIRandom.Next(1, 3) : _interval1Length[i-1];
			_interval2Length[i] = !(i != 0 && i % intervalContinuity != 1) ? Game.UIRandom.Next(1, 3) : _interval1Length[i-1];
			_interval3Length[i] = !(i != 0 && i % intervalContinuity != 2) ? Game.UIRandom.Next(1, 3) : _interval1Length[i-1];
		}

	}

	public TimerUI(int width)
        {
		_width = width;
		_internalWidth = width - 2;
		_intervalTimes = new double[_internalWidth+1];
		_interval1Length = new int[_internalWidth + 1];
		_interval2Length = new int[_internalWidth + 1];
		_interval3Length = new int[_internalWidth + 1];
	}

	public void UpdateTime(long dt) { }

	public bool NeedRefresh() => _needRefresh;

	protected override Measurement Measure(RenderContext context, int maxWidth)
        {
		return new Measurement(_width, _width);
	}

    protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
    {
		var timerText = !Game.IsDebugMode ? "" :
			string.Format(_timerText, _currentTime, _totalTime);

		yield return new Segment(ConstantsUI.BorderChar, Palette.InternalBorderStyle);

		var p = _currentWidthPosition;
		var currentWidth = _intervalTimes[p];
		var interval1Length = _interval1Length[p];
		var interval2Length = _interval2Length[p];
		var interval3Length = _interval3Length[p];

		for (var i = 0; i < _internalWidth; i++)
		{
			var currentChar = (i < timerText.Length) ? timerText[i].ToString() : " ";
			if (i < currentWidth - 3)
				yield return new Segment(currentChar, Palette.FirePalette.BkgStyleColor2);
			else if (i < currentWidth - interval1Length)
				yield return new Segment(currentChar, Palette.FirePalette.BkgStyleColor3);
			else if (i < currentWidth + 1)
				yield return new Segment(currentChar, Palette.FirePalette.BkgStyleColor4);
			else if (i <= currentWidth + interval2Length + 1)
				yield return new Segment(currentChar, Palette.FirePalette.BkgStyleColor6);
			else if (i <= currentWidth + interval2Length + interval3Length + 1)
				yield return new Segment(currentChar, Palette.FirePalette.BkgStyleColor7);
			else
				yield return new Segment(currentChar, Palette.FirePalette.BkgStyleColor8);
		}
		yield return new Segment(ConstantsUI.BorderChar, Palette.InternalBorderStyle);
	}
    }
