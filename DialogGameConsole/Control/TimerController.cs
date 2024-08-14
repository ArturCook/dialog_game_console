using DialogGameConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Control;

public class TimerController
{
	private readonly TimerUI _ui;

	private int _totalTime = 40 * 1000;

	private long _currentTime;

	public TimerController(TimerUI ui)
	{
		_ui = ui;
		ResetTimer();
	}

	public void ResetTimer()
	{
		_currentTime = _totalTime;
		_ui.ResetTimer(_totalTime);
	}

	public void UpdateTime(long dt)
	{
		_currentTime -= dt;
		if (_currentTime <= 0)
			ResetTimer();
		else
			_ui.SetSeconds(_currentTime);
	}
}
