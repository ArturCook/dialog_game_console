using DialogGameConsole.Util;
using System;

namespace DialogGameConsole.Infos.Interfaces;

public interface IInfoValue
{
    IInfo GetInfo();
    object GetValue();
    object GetDefaultValue();
    bool IsType(Type type);
}