using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using System;
using System.Collections.Generic;

namespace DialogGameConsole.Infos;

public abstract class MentalMap : InfoMap
{
    public abstract IInfoGroup Branching { get; }

    public abstract IInfoGroup Generic { get; }

    public override IEnumerable<IInfoGroup> GetGroups()
    {
        yield return Branching;
        yield return Generic;
    }
}

