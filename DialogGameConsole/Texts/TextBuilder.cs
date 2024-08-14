using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Text.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;

namespace DialogGameConsole.Texts;
public class TextBuilder
{
    private string? _defaultText;

    private Character _character = Character.Undefined;

    private Delay _delay = Delay.Undefined;

    private List<(string, InfoPredicate)> _texts = new();

    private List<(Delay, InfoPredicate)> _status = new();

    public TextBuilder(params object[] args) {
        if (args is null || args.Length < 2)
            throw new ArgumentException("No sufficient arguments passed to option builder");

        _ = ParseObject(args[0], args[1..]);
    }

    public static bool CanParse(object[] args) {
        try {
            _ = new TextBuilder(args);
        }
        catch(ArgumentException) {
            return false;
        }
        return true;
    }


    private object ParseObject(object argObject, object[] nextArgs) {
        var nextArgObject = nextArgs.Length == 0 ? null
            : ParseObject(nextArgs[0], nextArgs[1..]);
        switch (argObject, nextArgObject) {
            case (string arg1, null):
                Assert.IsNull(_defaultText, nameof(_defaultText));
                _defaultText = arg1;
                return null;
            case (string arg1, InfoPredicate arg2):
                _texts.Add((arg1, arg2));
                return null;
            case (Character arg1, null):
                Assert.IsDefault(_character, nameof(_character));
                _character = arg1;
                return null;
            case (Delay arg1, null):
                Assert.IsDefault(_delay, nameof(_delay));
                _delay = arg1;
                return null;
            case (Delay arg1, InfoPredicate arg2):
                _status.Add((arg1, arg2));
                return null;
            case (InfoPredicate arg1, null):
                return arg1;
            case (InfoPredicate arg1, InfoPredicate arg2):
                return arg1.And(arg2);
            case (IInfo arg1, Enum arg2):
                return arg1.Is(arg2);
            case (IInfo arg1, List<Enum> arg2):
                return arg1.Is(arg2.ToArray());
            case (Enum arg1, null):
                return arg1;
            case (Enum arg1, Enum arg2):
                return new List<Enum> { arg1, arg2 };
            case (Enum arg1, List<Enum> arg2):
                return new List<Enum>(arg2) { arg1 };
        }
        throw new ArgumentException("Failure Parsing");

    }

    public DialogText Build() {
        var defaultDelay = _delay == Delay.Undefined ? Delay.Normal : _delay;
        var newText = new DialogText(_character, _defaultText, defaultDelay);

        foreach (var (text, predicate) in _texts) {
            newText.AddText(text, predicate);
        }
        foreach (var (delay, predicate) in _status) {
            newText.AddDelay(delay, predicate);
        }
        return newText;
    }
}
