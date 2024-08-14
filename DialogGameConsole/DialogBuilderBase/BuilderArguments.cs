using DialogGameConsole.Enums;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Text.Enums;
using System;
using System.Collections.Generic;

namespace DialogGameConsole.DialogBuilderBase;

public class BuilderArguments
{
    public string CurrentText => _values[typeof(string)];
    public Character MessageSource => _values[typeof(Character)];
    public Delay CurrentTiming => _values[typeof(Delay)];
    public OptionStatus OptionStatus => _values[typeof(OptionStatus)];
    public OptionDelay OptionTiming => _values[typeof(OptionDelay)];
    public CMD DialogCommand => _values[typeof(CMD)];

    public readonly Dictionary<Type, dynamic> _values = new();

    public void SetValue(object argument)
    {
        var type = argument.GetType();
        if (!_values.ContainsKey(type))
            throw new ArgumentException($"Type {type} is not valid");
        _values[type] = argument;
    }

    public void Reset()
    {
        _values[typeof(string)] = null;
        _values[typeof(Character)] = _values.GetValueOrDefault(typeof(Character), Character.Undefined);
        _values[typeof(Delay)] = Delay.Immediate;
        _values[typeof(OptionStatus)] = OptionStatus.Normal;
        _values[typeof(OptionDelay)] = OptionDelay.Immediate;
        _values[typeof(CMD)] = CMD.None;
    }

    public BuilderArguments()
    {
        Reset();
        _values[typeof(Character)] = Character.Undefined;
    }

    //public DialogNetworkBuilder<T> Add<T>(this DialogNetworkBuilder<T> builder, params object[] arguments)
    //{
    //    var args = new BuilderArguments();

    //    foreach (var argument in arguments)
    //    {
    //        var currentCommand = args.DialogCommand;
    //        var nextCommand = currentCommand;
    //        var finishCommand = false;
    //        if (argument is CMD value && value != CMD.None)
    //        {
    //            nextCommand = value;
    //        }
    //        else if (argument is OP value2 && value2 != OP.Undefined)
    //        {
    //            nextCommand = CMD.Option;
    //            finishCommand = true;
    //        }
    //        else if (argument is string && args.DialogCommand == CMD.None)
    //        {
    //            nextCommand = CMD.Dialog;
    //        }
    //        else if (argument is string && args.DialogCommand == CMD.Dialog)
    //        {
    //            finishCommand = true;
    //        }
    //        finishCommand = finishCommand || currentCommand != CMD.None
    //            && currentCommand != nextCommand;

    //        if (finishCommand)
    //        {
    //            if (currentCommand is CMD.MergeBranches)
    //            {
    //                builder = builder.MergeAllBranches();
    //            }
    //            else if (currentCommand is CMD.FinishDialog)
    //            {
    //                builder = builder.FinishDialog();
    //            }
    //            else if (currentCommand is CMD.Option)
    //            {
    //                builder = builder.AddOption(args.OptionType, args.CurrentText, args.OptionStatus, args.OptionTiming);
    //            }
    //            else if (currentCommand is CMD.Dialog)
    //            {
    //                if (args.MessageSource == DS.Undefined)
    //                    throw new ArgumentException("Message source undefined");
    //                if (args.MessageSource == DS.Player)
    //                    builder = builder.AddPlayerText(args.CurrentText, args.CurrentTiming);
    //                if (args.MessageSource == DS.Npc)
    //                    builder = builder.AddNpcText(args.CurrentText, args.CurrentTiming);
    //            }
    //            else
    //            {
    //                throw new ArgumentException("Previous Command not identified");
    //            }
    //            args.Reset();
    //        }
    //        args.SetValue(argument);
    //        args.SetValue(nextCommand);
    //    }
    //    if (args.DialogCommand is CMD.FinishDialog)
    //    {
    //        builder = builder.FinishDialog();
    //    }
    //    return builder;
    //}
}