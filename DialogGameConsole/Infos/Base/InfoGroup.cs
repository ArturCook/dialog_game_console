using DialogGameConsole.Infos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DialogGameConsole.Infos.Base;

public abstract class InfoGroup<T> : IInfoGroup
{
    private readonly Dictionary<IInfo, object> _fieldValues = new();

    private static readonly Action<InfoGroup<T>> _init;

    public bool HasInfo(IInfo info)
    {
        return _fieldValues.ContainsKey(info);
    }

    public void SetValue(IInfoValue infoValue, bool forceValue = false)
    {
        AssertInfoExists(infoValue.GetInfo());
        var currentValue = _fieldValues[infoValue.GetInfo()];
        if (!forceValue && !currentValue.Equals(infoValue.GetDefaultValue()))
            throw new ArgumentException($"Can't accept infovalue: {infoValue} because its value has already been set: {currentValue}");
        _fieldValues[infoValue.GetInfo()] = infoValue.GetValue();
    }

    public void SetValue<K>(Info<K> info, K value, bool forceValue = false) where K : struct
    {
        SetValue(info.GetInfoValue(value), forceValue);
    }

    public K GetValue<K>(Info<K> info) where K : struct
    {
        AssertInfoExists(info);
        return (K)_fieldValues[info];
    }

    public K GetValue<K>(IInfo info) where K : struct
    {
        AssertInfoExists(info);
        return (K)_fieldValues[info];
    }

    public IInfoValue GetInfoValue(IInfo info)
    {
        return info.GetInfoValue(_fieldValues[info]);
    }

    private void AssertInfoExists(IInfo info)
    {
        if (!_fieldValues.ContainsKey(info))
            throw new ArgumentException($"Info {info} not present on Group {this}");
    }

    public InfoGroup()
    {
        _init(this);
    }

    static InfoGroup()
    {
        // Using reflection here bacause it's only used in a static context
        var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(field => field.FieldType.GetInterface(nameof(IInfo)) is not null);

        var setterList = new List<(FieldInfo Field, IInfo Info, Enum DefaultValue)>();
        foreach (var field in fields)
        {
            var newInfo = (IInfo)Activator.CreateInstance(field.FieldType, typeof(T), field.Name);
            var defaultValue = (Enum)Activator.CreateInstance(newInfo.ValueType());
            setterList.Add((field, newInfo, defaultValue));
        }
        _init = new Action<InfoGroup<T>>((instance) =>
        {
            foreach (var pair in setterList)
            {
                pair.Field.SetValue(instance, pair.Info);
                instance._fieldValues[pair.Info] = pair.DefaultValue;
            }
        });
    }
}