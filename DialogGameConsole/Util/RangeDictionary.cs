using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Util;
public class RangeDictionary<T>
{
    private readonly Dictionary<Func<long, bool>, T> _dictionary = new();

    public RangeDictionary() { 
    
    }

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(long from, long to, T value) {
        _dictionary.Add(d => d >= from && d < to, value);
    }

    public void AddRange(IEnumerable<(long length, T value)> values) {
        Clear();
        Add(long.MinValue, 0, default(T));
        long totalLength = 0;
        foreach (var value in values) {
            var length = value.length;
            Add(totalLength, totalLength + length, value.value);
            totalLength += length;
        }
        Add(totalLength, long.MaxValue, default(T));
    }

    public void Clear() => _dictionary.Clear();
    
    public T Get(long value) {
        return _dictionary.Single(x => x.Key(value)).Value;
    }
}
