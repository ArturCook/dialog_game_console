using DialogGameConsole.Enums;
using DialogGameConsole.Infos;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Options;
using System;

namespace DialogGameConsole.DialogBase;

public interface IDialog
{
    public abstract DialogNetwork Network { get; }

    public abstract MentalMap Map { get; }

    public abstract SubjectMap SubjectMap { get; }

    public DialogOptionInstance NewOptionInstance(DialogOption options, Action onTrigger);

    public abstract DialogOption NewOption(string baseText = "");
}

public abstract class Dialog<T, K> : IDialog where T : MentalMap, new()
    where K : SubjectMap, new()
{
    public DialogNetwork Network { get; } 

    MentalMap IDialog.Map => Map;

    public T Map { get; }

    SubjectMap IDialog.SubjectMap => SubjectMap;

    public K SubjectMap { get; } 

    private CombinedInfoMap _combinedMap;

    public DialogOptionInstance NewOptionInstance(DialogOption options, Action onTrigger) => new(_combinedMap, options, onTrigger);

    public DialogOption NewOption(string baseText = "") => new(baseText);

    public Dialog() {
        Network = new();

        Map = new T();
        SubjectMap = new K();
        _combinedMap = new(Map, SubjectMap);
    }
}