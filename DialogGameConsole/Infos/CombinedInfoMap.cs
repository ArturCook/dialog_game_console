using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Infos;

public class CombinedInfoMap : InfoMap
{
    private InfoMap[] _maps;

    public override IEnumerable<IInfoGroup> GetGroups() {
        return _maps.SelectMany(map => map.GetGroups());
    }
    
    public CombinedInfoMap(params InfoMap[] maps) {
        _maps = maps;
    }
}
