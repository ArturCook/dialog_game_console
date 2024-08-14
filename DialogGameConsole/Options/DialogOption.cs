using DialogGameConsole.DialogBase;
using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Options;

public class DialogOption : DialogBase<DialogOption>
{
    private readonly PropertyWithPredicate<string> _text;
    private readonly PropertyWithPredicate<OptionStatus> _status;
    private readonly PropertyWithPredicate<OptionPriority> _priority;
    private readonly PropertyWithPredicate<OptionDelay> _delay;
    private readonly PropertyWithPredicate<OptionSpeed> _speed;

    public string GetText(InfoMap map) => _text.Get(map);
    public OptionStatus GetStatus(InfoMap map) => _status.Get(map);
    public OptionPriority GetPriority(InfoMap map) => _priority.Get(map);
    public OptionDelay GetDelay(InfoMap map) => _delay.Get(map);
    public OptionSpeed GetSpeed(InfoMap map) => _speed.Get(map);

    public void AddText(string text, IInfoPredicate predicate) => _text.AddVariation(text, predicate);
    public void AddCondition(OptionStatus status, IInfoPredicate predicate) => _status.AddVariation(status, predicate);
    public void AddPriority(OptionPriority status, IInfoPredicate predicate) => _priority.AddVariation(status, predicate);
    public void AddDelay(OptionDelay status, IInfoPredicate predicate) => _delay.AddVariation(status, predicate);
    public void AddSpeed(OptionSpeed status, IInfoPredicate predicate) => _speed.AddVariation(status, predicate);

    internal DialogOption(string baseText, OptionPriority? defaultPriority = null,
        OptionDelay? defaultDelay = null, OptionSpeed? defaultSpeed = null) {

        Assert.IsNotNull(baseText, nameof(baseText));
        _text = new(baseText, (_, v2) => v2);
        _status = new(OptionStatus.Normal, (s1, s2) => s1.Combine(s2));
        _priority = new(defaultPriority ?? OptionPriority.Normal, (s1, s2) => s1.Combine(s2));
        _delay = new(defaultDelay ?? OptionDelay.Normal, (s1, s2) => s1.Combine(s2));
        _speed = new(defaultSpeed ?? OptionSpeed.Normal, (s1, s2) => s1.Combine(s2));
    }

    public override string ToString() {
        return _text.ToString() + "\n" + _status.ToString();
    }
}
