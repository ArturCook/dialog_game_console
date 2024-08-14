using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Util;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
    {
        while (count-- > 0)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }

    public static List<T> UnionWith<T>(this IEnumerable<T> source, T element) {
        var newList = new List<T>(source);
        newList.Add(element);
        return newList;
    }

    public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((x, i) => (i, x));
    }

    public static IEnumerable<List<TSource>> BreakAt<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
        var (x, y) = (new List<TSource>(), source);
        while(y.Any()) {
            (x, y) = y.BreakAtSingle(predicate);
            yield return x;
        }
    }

    public static (List<TSource>, IEnumerable<TSource>) BreakAtSingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
        return (source.TakeWhile((x, i) => i==0 || !predicate(x)).ToList(), source.SkipWhile((x, i) => i == 0 || !predicate(x)));
    }
}
