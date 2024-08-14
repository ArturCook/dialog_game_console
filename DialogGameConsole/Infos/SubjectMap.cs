using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using System.Collections.Generic;

namespace DialogGameConsole.Infos;

public abstract class SubjectMap : InfoMap
{
    public abstract IInfoGroup Subject { get; }

    public override IEnumerable<IInfoGroup> GetGroups() {
        yield return Subject;
    }
}

