using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Util;
using System;

namespace DialogGameConsole.Infos.Base;

public class InfoValue<T> : IInfoValue where T : struct
{
    public int Id { get; } = Ids.GetId(typeof(IInfoValue));

    public Info<T> Info;

    public T Value;

    public IInfo GetInfo() => Info;

    public object GetValue() => Value;

    public object GetDefaultValue() => default(T);

    public bool IsType(Type type) => Info.ParentType == type;

    public InfoValue(Info<T> info, T value)
    {
        Assert.IsNotNull(info, nameof(info));
        Assert.IsNotNull(value, nameof(value));
        Info = info;
        Value = value;
    }

    public override string ToString()
    {
        return $"'{Info}', Value: '{Value}'";
    }
}

