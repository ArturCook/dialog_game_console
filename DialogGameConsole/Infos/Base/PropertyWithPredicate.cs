using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Infos.Base;

public class PropertyWithPredicate<T>
{
    private readonly List<(T, IInfoPredicate)> _text = new();

    private readonly T _defaultValue;

    private readonly Func<T, T, T> _joinFunction;

    public T Get(InfoMap map) {
        var value = _defaultValue;
        foreach (var (newValue, isValid) in _text) {
            if (isValid.Eval(map))
                value = _joinFunction(value, newValue);
        }
        return value;
    }

    public void AddVariation(T value, IInfoPredicate predicate) {
        Assert.IsNotNull(predicate, nameof(predicate));
        Assert.IsNotNull(value, nameof(value));

        if (value.Equals(default(T)))
            throw new Exception($"Value cannot be default of type {typeof(T).Name}");

        _text.Insert(0, (value, predicate));
    }

    public override string ToString() {
        if (!_text.Any())
            return $"\"{_defaultValue}\"";
        return string.Join("\n", _text.Select(s => $"\"{s.Item1}\" if ({s.Item2})"))
            + $"\n\"{_defaultValue}\" else";
    }

    public PropertyWithPredicate(T defaultValue, Func<T, T, T> joinFunction) {
        _defaultValue = defaultValue;
        _joinFunction = joinFunction;
    }

}