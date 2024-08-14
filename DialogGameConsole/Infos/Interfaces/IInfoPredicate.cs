using DialogGameConsole.Infos.Base;
using System;

namespace DialogGameConsole.Infos.Interfaces;

public interface IInfoPredicate
{
    bool Eval(InfoMap map);
}