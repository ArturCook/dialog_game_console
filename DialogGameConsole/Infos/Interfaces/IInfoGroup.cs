using DialogGameConsole.Infos.Base;

namespace DialogGameConsole.Infos.Interfaces;

public interface IInfoGroup
{
    bool HasInfo(IInfo infoValue);
    void SetValue(IInfoValue infoValue, bool forceValue = false);
    void SetValue<K>(Info<K> info, K value, bool forceValue = false) where K : struct;
    K GetValue<K>(Info<K> info) where K : struct;
    K GetValue<K>(IInfo info) where K : struct;
    IInfoValue GetInfoValue(IInfo info);
}