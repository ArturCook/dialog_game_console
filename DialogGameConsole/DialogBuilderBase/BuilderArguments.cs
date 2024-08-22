using DialogGameConsole.Enums;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Text.Enums;
using System;
using System.Collections.Generic;

namespace DialogGameConsole.DialogBuilderBase;

public class BuilderArguments
{
    public string CurrentText => _values[typeof(string)];
    public Character MessageSource => _values[typeof(Character)];
    public Delay CurrentTiming => _values[typeof(Delay)];
    public OptionStatus OptionStatus => _values[typeof(OptionStatus)];
    public OptionDelay OptionTiming => _values[typeof(OptionDelay)];
    public CMD DialogCommand => _values[typeof(CMD)];

    public readonly Dictionary<Type, dynamic> _values = new();

    public void SetValue(object argument)
    {
        var type = argument.GetType();
        if (!_values.ContainsKey(type))
            throw new ArgumentException($"Type {type} is not valid");
        _values[type] = argument;
    }

    public void Reset()
    {
        _values[typeof(string)] = null;
        _values[typeof(Character)] = _values.GetValueOrDefault(typeof(Character), Character.Undefined);
        _values[typeof(Delay)] = Delay.Immediate;
        _values[typeof(OptionStatus)] = OptionStatus.Normal;
        _values[typeof(OptionDelay)] = OptionDelay.Immediate;
        _values[typeof(CMD)] = CMD.None;
    }

    public BuilderArguments()
    {
        Reset();
        _values[typeof(Character)] = Character.Undefined;
    }

}