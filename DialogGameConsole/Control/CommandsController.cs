using System;

namespace DialogGameConsole.Control;

public class CommandsController
{
	private ConsoleKey? _keyBuffer = null;

	private readonly OptionsController _optionsController;

	public CommandsController(OptionsController optionsController)
    {
		_optionsController = optionsController;
    }

    public void HandleKeyPress(ConsoleKey key)
    {
		_keyBuffer = key;
	}

	public void ExcuteCurrentCommand(Game game)
        {

		if (!_keyBuffer.HasValue) return;
		var key = _keyBuffer.Value;
		if (key == ConsoleKey.UpArrow)
		{
			_optionsController.Decrement();
		}
		else if (key == ConsoleKey.DownArrow)
		{
			_optionsController.Increment();
		}
		else if (key == ConsoleKey.Enter)
		{
			_optionsController.SelectCurrentOption();
		}
		else if (key == ConsoleKey.D)
		{
			game.ToggleDebug();
		}
		else if (key == ConsoleKey.Q)
		{
			game.Finish();
		}
		else if (key == ConsoleKey.R)
		{
			game.Rewind();
		}
		else if (key == ConsoleKey.S)
		{
			game.UpdateSpeed();
		}
		_keyBuffer = null;
	}
    }
