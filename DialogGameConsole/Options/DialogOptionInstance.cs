using DialogGameConsole.Enums;
using DialogGameConsole.Infos.Base;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Options;

public class DialogOptionInstance
{
    private readonly InfoMap _map;
    public DialogOption _option { get; }

    private readonly long _totalTime;
    
    private long _remainingTime;

    private readonly bool _isEmpty;

    public Action Action { get; }
    public string Text { get; private set; }
    public OptionStatus Status { get; private set; }

    public int GetOptionId() => _option.Id;

    public void UpdateTime(long dt) {
        if (_isEmpty)
            return;

        if (Status != OptionStatus.Loading)
            return;
        _remainingTime -= dt;
        _remainingTime = Math.Clamp(_remainingTime, 0, _totalTime);
        if (_remainingTime == 0)
            Update();
    }

    public void AddTime(int dt) {
        if (_isEmpty)
            return;

        _remainingTime += dt;
        Update();
    }

    public void Update() {
        Text = _option.GetText(_map);
        Status = _option.GetStatus(_map);
        if (Status == OptionStatus.Normal && _remainingTime > 0)
            Status = OptionStatus.Loading;
    }

    public bool IsEmpty() => _isEmpty;

    public List<(long, bool)> GetDelayProfile() {
        if (_isEmpty)
            return new();

        var delay = _option.GetDelay(_map);
        return delay switch {
            OptionDelay.Immediate => new() { ((long)50, false) },
            //OptionDelay.Fastest => new[] { (long)500 },
            //OptionDelay.Faster => new[] { (long)500 },
            OptionDelay.Fast => new() { ((long)500, true), (100, false), (500, true) },
            OptionDelay.Normal => new() { ((long)1500, true), (100, false), (1500, true) },
            OptionDelay.Slow => new() { ((long)2500, true), (100, false), (2500, true) },
            //OptionDelay.Slower => new[] { (long)7000 },
            //OptionDelay.Slowest => new[] { (long)10000 },

            _ => throw new ArgumentException($"Profile of delay {delay} is not implemented")
        };
    }

    public double GetSpeed() {
        if (_isEmpty)
            return 1;

        var speed = _option.GetSpeed(_map);
        return speed switch {
            OptionSpeed.Normal => 1,
            OptionSpeed.Fast => 2,
            OptionSpeed.Slow => 0.5,
            
            _ => throw new ArgumentException($"Profile of delay {speed} is not implemented")
        };
    }

    public long GetTotalTime(InfoMap map) {
        if (_isEmpty)
            return 0;

        return GetDelayProfile().ToList().Select(x => x.Item1).Sum();
    }

    public static DialogOptionInstance NewEmptyInstance() => new(null, null, () => { }, true);

    public DialogOptionInstance(InfoMap map, DialogOption dialogOption, Action action)
        : this(map, dialogOption, action, false) {
        Assert.IsNotNull(map, nameof(map));
        Assert.IsNotNull(dialogOption, nameof(dialogOption));
        Assert.IsNotNull(action, nameof(action));
    }

    private DialogOptionInstance(InfoMap map, DialogOption dialogOption, Action action, bool isEmpty) {
        _map = map;
        _option = dialogOption;
        _isEmpty = isEmpty;
        Action = action;

        _totalTime = GetTotalTime(map);
        _remainingTime = _totalTime;
    }
}
