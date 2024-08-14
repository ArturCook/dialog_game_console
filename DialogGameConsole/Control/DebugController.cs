using DialogGameConsole.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Control;

public class DebugController
{
	private readonly DebugUI _ui;

    private bool _isEnabled;

    private int _messageCount = 0;

    private List<string> _debugMessages = new();

	public DebugController(DebugUI ui, bool isEnabled)
	{
		_ui = ui;
        _isEnabled = isEnabled;
	}

    public void AddMessage(string message) {
        _messageCount++;
        _debugMessages.Add(message);
        _ui.AddMessage(_messageCount, message);
    }

    public void Set(bool isDebugMode)
    {
        _isEnabled = isDebugMode;
        _ui.Set(_isEnabled);
    }
}
