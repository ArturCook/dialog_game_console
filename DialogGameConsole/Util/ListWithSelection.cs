using System;
using System.Collections;
using System.Collections.Generic;

namespace DialogGameConsole.Util;

public class ListWithSelection<T> : IEnumerable<T>
{
    private int _index = 0;

    private readonly int _maxOptions;

    private readonly List<T> _list;

    public void Increment() => IncrementOptions(1);

    public void Decrement() => IncrementOptions(-1);

    public void IncrementOptions(int n)
    {
        _index = (_index + Math.Sign(n) + _maxOptions) % _maxOptions;
    }

    public void Add(T value)
    {
        Assert.IsNotNull(value, nameof(value));
        if (_list.Count >= _maxOptions)
            throw new InvalidOperationException($"List is already on max capacity: {_maxOptions}");
        if (_list.Contains(value))
            throw new InvalidOperationException($"List already contains value: {value}");
        _list.Add(value);
    }

    public void Remove(T value)
    {
        var index = _list.IndexOf(value);
        if (index == -1)
            throw new InvalidOperationException($"Value {value} is not on the list");
        RemoveAt(index);
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
        if (_list.Count == 0)
            index = -1;
        if (index == 0 && _index >= _list.Count)
            _index--;
    }

    public int GetSelectedIndex()
    {
        return _index;
    }

    public T GetSelected()
    {
        return Get(_index);
    }

    public T Get(int index)
    {
        if (_index < 0 || _index >= _list.Count) return default;
        return _list[index];
    }

    internal void Clear()
    {
        _list.Clear();
    }

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    public ListWithSelection(int maxOptions)
    {
        _maxOptions = maxOptions;
        _list = new(_maxOptions);
    }
}
