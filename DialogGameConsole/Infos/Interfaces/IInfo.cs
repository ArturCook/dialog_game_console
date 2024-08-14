using DialogGameConsole.Infos.Base;
using System;

namespace DialogGameConsole.Infos.Interfaces;

public interface IInfo
{
    string Name { get; }
    bool IsSameType<K>() where K : struct;
    bool IsSameType(Type type);
    Type ValueType();
    IInfoValue GetInfoValue(object value);
    public InfoPredicate Is(object value);
    public InfoPredicate IsNot(object value);

}
