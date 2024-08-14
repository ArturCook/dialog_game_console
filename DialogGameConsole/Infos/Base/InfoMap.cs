using DialogGameConsole.Infos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Infos.Base;

public abstract class InfoMap
{
    private Stack<IInfoValue> _history = new();
    public abstract IEnumerable<IInfoGroup> GetGroups();

    public void SetValue(IInfoValue infoValue, bool forceValue = false) {
        foreach (var i in GetGroups()) {
            if (!infoValue.IsType(i.GetType()))
                continue;

            var previousValue = i.GetInfoValue(infoValue.GetInfo());
            _history.Push(previousValue);
            i.SetValue(infoValue, forceValue);
        }
    }

    public K GetValue<K>(Info<K> info) where K : struct
        => GetValueInternal<K>(info);

    public K GetValue<K>(IInfo info) where K : struct
        => GetValueInternal<K>(info);

    public bool IsEqual<K>(IInfo info, K value) where K : struct
        => GetValueInternal<K>(info).Equals(value);

    public bool IsAnyEqual<K>(IInfo info, params K[] values) where K : struct
        => values.Any(value => IsEqual(info, value));

    public bool IsNotAnyEqual<K>(IInfo info, params K[] values) where K : struct
        => values.All(value => !IsEqual(info, value));

    private K GetValueInternal<K>(IInfo info) where K : struct {
        if (!info.IsSameType<K>())
            throw new ArgumentException($"Invalid type for info {info}");
        foreach (var group in GetGroups()) {
            if (!group.HasInfo(info))
                continue;
            return group.GetValue<K>(info);
        }
        throw new Exception($"Info {info} is not present on group {this}");
    }

    public void Rewind() {
        var previousValue = _history.Pop();
        SetValue(previousValue, true);
    }

}
