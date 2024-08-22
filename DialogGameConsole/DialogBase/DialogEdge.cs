using DialogGameConsole.Enums;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Options;
using DialogGameConsole.Text.Enums;
using DialogGameConsole.Texts;
using DialogGameConsole.Util;
using System;

namespace DialogGameConsole.DialogBase;

public class DialogEdge : DialogBase<DialogEdge>
{
    public EdgeType Type { get; }
    public DialogOption Option { get; init; }
    public DialogText Text { get; init; }
    public IInfoPredicate Predicate { get; init; }
    public double Probability { get; init; }

    public bool IsTiming() => Type == EdgeType.Timing;

    public bool IsDirect() => Type == EdgeType.Direct;

    public bool IsOption() => Type == EdgeType.Option;

    public bool IsProbability() => Type == EdgeType.Probability;

    public bool IsCondition() => Type == EdgeType.Condition;

    public static DialogEdge NewProbability(double probability)
    {
        Assert.IsWithin(probability, 0, 100, nameof(probability));
        return new DialogEdge(EdgeType.Probability)
        {
            Probability = probability
        };
    }

    public static DialogEdge NewDirect()
    {
        return new DialogEdge(EdgeType.Direct);
    }

    public static DialogEdge NewStop() {
        return new DialogEdge(EdgeType.Stop);
    }

    public static DialogEdge NewDelay(DialogText text)
    {
        Assert.IsNotNull(text, nameof(text));
        return new DialogEdge(EdgeType.Timing)
        {
            Text = text,
        };
    }

    public static DialogEdge NewOption(DialogOption option)
    {
        Assert.IsNotNull(option, nameof(option));
        return new DialogEdge(EdgeType.Option)
        {
            Option = option,
        };
    }

    public static DialogEdge NewCondition(IInfoPredicate predicate)
    {
        Assert.IsNotNull(predicate, nameof(predicate));
        return new DialogEdge(EdgeType.Option)
        {
            Predicate = predicate,
        };
    }

    private DialogEdge(EdgeType edgeType)
    {
        Type = edgeType;
    }

    public override string ToString()
    {
        var prefix = base.ToString() + " - " + Type.GetText();
        return Type switch
        {
            EdgeType.Timing => prefix + ", D: " + Text,
            EdgeType.Option => prefix + ", O: " + Option,
            EdgeType.Probability => prefix + ", P: " + Probability,
            EdgeType.Condition => prefix + ", C: " + Predicate,
            _ => prefix
        };
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(DialogEdge))
            return false;
        return Id == ((DialogEdge)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}