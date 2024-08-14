using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Util;
using System;

namespace DialogGameConsole.Infos.Base;

public class Info<T> : IInfo where T : struct
{
    public int Id { get; } = Ids.GetId(typeof(IInfo));
    public string Name { get; }
    public Type Type;
    public Type ParentType;
    public T Values;

    public bool IsSameType<K>() where K : struct 
        => IsSameType(typeof(K));

    public bool IsSameType(Type type)
        => Type == type;

    public Type ValueType() => Type;

    public InfoValue<T> GetInfoValue(T value) {
        return new(this, value);
    }

    IInfoValue IInfo.GetInfoValue(object value) => GetInfoValue((T)value);

    public InfoPredicate<T> Is(params T[] values) => InfoPredicate<T>.Is(this, values);

    public InfoPredicate Is(object value) => Is((T)value);

    public InfoPredicate<T> IsNot(params T[] values) => InfoPredicate<T>.IsNot(this, values);

    public InfoPredicate IsNot(object value) => IsNot((T)value);

    public Info(Type parentType, string fieldName) {
        Assert.IsNotNull(parentType, nameof(parentType));
        Assert.IsNotEmpty(fieldName, nameof(fieldName));

        Name = fieldName;
        Type = typeof(T);
        ParentType = parentType;
    }

    public override string ToString() {
        return $"{Name} ({ParentType.Name})";
    }

    

}

