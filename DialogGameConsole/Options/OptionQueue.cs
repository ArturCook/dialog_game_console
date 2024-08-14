using DialogGameConsole.Options.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Options;

public class OptionQueue
{
    private readonly Dictionary<int, DialogOptionInstance> _instances = new();

    public DialogOptionInstance TryGetInstance(DialogOption option) {
        Assert.IsNotNull(option, nameof(option));
        return _instances.GetValueOrDefault(option.Id);
    }

    public bool HasOption(DialogOption option) {
        Assert.IsNotNull(option, nameof(option));
        return _instances.ContainsKey(option.Id);
    }

    public void Add(DialogOptionInstance instance) {
        Assert.IsNotNull(instance, nameof(instance));
        var id = instance.GetOptionId();
        if (_instances.ContainsKey(id))
            throw new ArgumentException($"Id {id} is already on the list");

        _instances[id] = instance;
    }

    public void Remove(DialogOption option) {
        Remove(TryGetInstance(option) ?? throw new ArgumentException($"Option {option} is not on the list"));
    }

    public void Remove(DialogOptionInstance instance) {
        Assert.IsNotNull(instance, nameof(instance));
        var id = instance.GetOptionId();
        if (!_instances.ContainsKey(id))
            throw new ArgumentException($"Instance for option with id {id} is not on the list");

        _instances.Remove(id);
    }

    public void Update() {
        foreach (var option in _instances.Values.ToArray()) {
            option.Update();
        }
    }

    private IEnumerable<DialogOptionInstance> OrderedInstances() {
        return _instances.Values
            .Where(i => i.Status.IsVisible())
            .OrderBy(i => i.GetOptionId());
    }

    public IEnumerable<DialogOptionInstance> GetTop3() {
        int count = 0;
        foreach(var instance in OrderedInstances().Take(3)) {
            count++;
            yield return instance;
        }
        for (int i = count; i < 3; i++) {
            yield return DialogOptionInstance.NewEmptyInstance();
        }
    }
}
