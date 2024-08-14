using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Infos.Base;

public class InfoPredicate : IInfoPredicate {

    public Predicate<InfoMap> Predicate;

    internal string _text;

    public bool Eval(InfoMap map) {
        return Predicate(map);
    }


    public InfoPredicate And(InfoPredicate p2) {
        if(this == False || p2 == False) return False;
        if(this == True) return p2;
        if(p2 == True) return this;
        return new((model) => Predicate(model) && p2.Predicate(model), $"({_text} And {p2._text}");
    }

    public InfoPredicate Or(InfoPredicate p2) {
        if (this == True || p2 == True) return True;
        if (this == False) return p2;
        if (p2 == False) return this;
        return new((model) => Predicate(model) || p2.Predicate(model), $"({_text} And {p2._text}");
    }

    public InfoPredicate Not() {
        if(this == True) return False;
        if(this == False) return True;
        return new((model) => !Predicate(model), $"Not ({_text})");
    }

    public static InfoPredicate And(IEnumerable<InfoPredicate> predicates) {
        var finalPredicate = True;
        foreach (var predicate in predicates) {
            finalPredicate = finalPredicate.And(predicate);
        }
        return finalPredicate;
    }

    public static InfoPredicate Or(IEnumerable<InfoPredicate> predicates) {
        var finalPredicate = False;
        foreach (var predicate in predicates) {
            finalPredicate = finalPredicate.Or(predicate);
        }
        return finalPredicate;
    }

    public static InfoPredicate True = new((_) => true, "True");

    public static InfoPredicate False = new((_) => false, "False");

    public InfoPredicate(Predicate<InfoMap> predicate, string text) {
        Predicate = predicate;
        _text = text;
    }

    public override string ToString() {
        return _text;
    }
}

public class InfoPredicate<T> : InfoPredicate where T : struct
{
    private Info<T> _info;

    private bool _isInclusion;

    private T[] _values;

    public InfoPredicate<T> AndNot(T value) {
        if (_isInclusion)
            throw new ArgumentException($"Function {nameof(AndNot)} cannot be used with inclusion InfoPredicate<T>");
        return IsNot(_info, _values.UnionWith(value).ToArray());
    }

    public InfoPredicate<T> Or(T value) {
        if (!_isInclusion)
            throw new ArgumentException($"Function {nameof(AndNot)} cannot be used with exclusion InfoPredicate<T>");
        return Is(_info, _values.UnionWith(value).ToArray());
    }

    public static InfoPredicate<T> IsNot(Info<T> info, params T[] values)
        => new(info, values, false, (map) => map.IsNotAnyEqual(info, values), $"{info.Name} {JoinValues(false, values)}");

    public static InfoPredicate<T> Is(Info<T> info, params T[] values)
        => new(info, values, true, (map) => map.IsAnyEqual(info, values), $"{info.Name} {JoinValues(true, values)}");
    
    private InfoPredicate(Info<T> info, T[] values, bool isInclusion, Predicate<InfoMap> predicate, string text)
        : base(predicate, text) {

        if (values == null || values.Length == 0)
            throw new ArgumentException("Values array not provided");

        _isInclusion = isInclusion;
        _info = info;
        _values = values;
    }

    private static string JoinValues(bool isInclusion, T[] values) {
        if(!values.Any()) return "None";
        if (values.Length == 1 && isInclusion)
            return $"={values[0]}";
        if (values.Length == 1 && !isInclusion)
            return $"<>{values[0]}";
        if (isInclusion)
            return "in (" + string.Join(",", values) + ")";
        return "not in (" + string.Join(",", values) + ")";
    }

    public override string ToString() {
        return _text;
    }
}