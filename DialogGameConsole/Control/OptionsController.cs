using DialogGameConsole.Options;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.UI;
using DialogGameConsole.Util;
using System;
using System.Linq;

namespace DialogGameConsole.Control;

public class OptionsController
{
	private const int _maxOptions = 3;

	private readonly OptionQueue _queue = new();

	private readonly ListWithSelection<DialogOptionInstance> _dialogOptions = new(3);


	private readonly OptionsUI _ui;

	private bool[] _optionEnabled = new bool[_maxOptions];

	private long[] _optionTimer = new long[_maxOptions];

	private bool[] _optionLoading = new bool[_maxOptions];

	private double[] _optionSpeed = new double[_maxOptions];

	private RangeDictionary<bool>[] _optionLoadingProfile = new RangeDictionary<bool>[_maxOptions];

	public OptionsController(OptionsUI ui)
    {
		_ui = ui;
        for (int i = 0; i < _maxOptions; i++) {
			_dialogOptions.Add(DialogOptionInstance.NewEmptyInstance());
            _optionLoadingProfile[i] = new();
		}
	}

	public void Increment() => IncrementOptions(1);

	public void Decrement() => IncrementOptions(-1);

	private void IncrementOptions(int n)
	{
		_dialogOptions.IncrementOptions(n);
		_ui.SetSelected(_dialogOptions.GetSelectedIndex());
	}

	public void UpdateTime(long dt)
	{
		foreach (var (i, option) in _dialogOptions.WithIndex())
		{
			var wasLoading = option.Status.IsLoading();
			if (!wasLoading) continue;
			
			option.UpdateTime(dt);
			var isLoading = option.Status.IsLoading();
			_ui.SetOption(i, option.Text, option.Status);

			if (!isLoading) {
				_optionTimer[i] = 0;
				_optionEnabled[i] = false;
				_optionLoading[i] = false;
				_optionLoadingProfile[i].Clear();
				continue;
			}
            
			var wasTyping = _optionLoading[i];
			var speed = _optionSpeed[i];

			_optionTimer[i] += dt;
			var isTyping = _optionLoadingProfile[i].Get((long)(_optionTimer[i] * speed));
			_optionLoading[i] = isTyping;
			if (isTyping && !wasTyping) {
				_ui.EnableOptionTyping(i);
			} else if (!isTyping && wasTyping) {
				_ui.DisableOptionTyping(i);
			}
		
		}
	}

	public bool HasOption(DialogOption option) {
		return _queue.HasOption(option);
	}

	public void AddOption(DialogOptionInstance instance)
	{
		_queue.Add(instance);
		UpdateOptions();
	}

	public void RemoveOption(DialogOption option) {
		_queue.Remove(option);
		UpdateOptions();
	}

	public void SelectCurrentOption()
	{
		var option = _dialogOptions.GetSelected();
		if (option.Status != OptionStatus.Normal) return;
		option.Action();
		_queue.Remove(option);
		UpdateOptions();

	}

	public bool IsObsolete(DialogOption option) {
		return _queue.TryGetInstance(option)?.Status.IsObsolete() ?? true;
	}

	public void UpdateOptions()
	{
		_queue.Update();
		var currentOptions = _dialogOptions.ToArray();
		var top3 = _queue.GetTop3().ToList();

		_dialogOptions.Clear();
		foreach (var i in Enumerable.Range(0, 3)) {
			var currentInstance = currentOptions[i];
			var newInstance = top3[i];

			_dialogOptions.Add(newInstance);
			if (currentInstance == newInstance) continue;
			UpdateOption(i, newInstance);
		}
	}

	public void UpdateOption(int i, DialogOptionInstance instance) {

		if (instance.IsEmpty()) {
			_ui.RemoveOption(i);
			_ui.SetOption(i, "", OptionStatus.Empty);
			return;
		}

		if (!instance.Status.IsNormal())
			instance.AddTime(300);

		var profile = instance.GetDelayProfile();
        _optionSpeed[i] = instance.GetSpeed();

		_optionEnabled[i] = instance.Status.IsLoading();
		_optionLoadingProfile[i].Clear();
		_optionLoadingProfile[i].AddRange(profile);

		_ui.SetOption(i, instance.Text, instance.Status);
	}
}
