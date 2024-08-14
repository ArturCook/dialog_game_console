using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;

namespace DialogGameConsole.Options;
public class OptionBuilder
{
    private string? _defaultText;

    private OptionPriority? _defaultPriority;

    private OptionDelay? _defaultDelay;

    private OptionSpeed? _defaultSpeed;

    private List<(OptionStatus, InfoPredicate)> _status = new();

    private List<(OptionPriority, InfoPredicate)> _priority = new();

    private List<(OptionDelay, InfoPredicate)> _delay = new();

    private List<(OptionSpeed, InfoPredicate)> _speed = new();

    private List<(string, InfoPredicate)> _texts = new();

    public OptionBuilder(params object[] args) {
        if (args is null || args.Length == 0) 
            throw new ArgumentException("No Arguments passed to option builder");

        _ = ParseObject(args[0], args[1..]);
    }

    private object ParseObject(object argObject, object[] nextArgs) {
        var nextArgObject = nextArgs.Length == 0 ? null
            : ParseObject(nextArgs[0], nextArgs[1..]);
        switch (argObject, nextArgObject) {
            case (string arg1, null):
                Assert.IsNull(_defaultText, nameof(_defaultText));
                _defaultText = arg1;
                return null;
            case (string arg1, InfoPredicate arg2):
                _texts.Add((arg1, arg2));
                return null;

            case (OptionStatus arg1, InfoPredicate arg2):
                _status.Add((arg1, arg2));
                return null;
            case (OptionPriority arg1, InfoPredicate arg2):
                _priority.Add((arg1, arg2));
                return null;
            case (OptionDelay arg1, InfoPredicate arg2):
                _delay.Add((arg1, arg2));
                return null;
            case (OptionSpeed arg1, InfoPredicate arg2):
                _speed.Add((arg1, arg2));
                return null;

            case (OptionPriority arg1, null):
                Assert.IsNull(_defaultPriority, nameof(_defaultPriority));
                _defaultPriority = arg1;
                return null;
            case (OptionDelay arg1, null):
                Assert.IsNull(_defaultDelay, nameof(_defaultDelay));
                _defaultDelay = arg1;
                return null;
            case (OptionSpeed arg1, null):
                Assert.IsNull(_defaultSpeed, nameof(_defaultSpeed));
                _defaultSpeed = arg1;
                return null;

            case (InfoPredicate arg1, null):
                return arg1;
            case (InfoPredicate arg1, InfoPredicate arg2):
                return arg1.And(arg2);
            case (IInfo arg1, Enum arg2):
                return arg1.Is(arg2);
            case (IInfo arg1, List<Enum> arg2):
                return arg1.Is(arg2.ToArray());
            case (Enum arg1, null):
                return arg1;
            case (Enum arg1, Enum arg2):
                return new List<Enum> { arg1, arg2 };
            case (Enum arg1, List<Enum> arg2):
                return new List<Enum>(arg2) { arg1 };
        }
        throw new Exception("Failure Parsing");
    }

    public static OptionBuilder New(params object[] args) => new OptionBuilder(args);

    public DialogOption Build() {
        var option = new DialogOption(_defaultText, _defaultPriority, _defaultDelay, _defaultSpeed);
        foreach (var (text, predicate) in _texts) {
            option.AddText(text, predicate);
        }
        foreach (var (status, predicate) in _status) {
            option.AddCondition(status, predicate);
        }
        foreach (var (priority, predicate) in _priority) {
            option.AddPriority(priority, predicate);
        }
        foreach (var (delay, predicate) in _delay) {
            option.AddDelay(delay, predicate);
        }
        foreach (var (speed, predicate) in _speed) {
            option.AddSpeed(speed, predicate);
        }
        return option;
    }
}
