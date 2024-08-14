using DialogGameConsole.Enums;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Text.Enums;
using DialogGameConsole.Texts;
using DialogGameConsole.Util;
using System;

namespace DialogGameConsole.DialogBase;

public class DialogNode : DialogBase<DialogNode>
{
    public NodeType Type { get; }
    public DialogText Text { get; init; }
    public IInfoValue InfoValue { get; init; }

    public static DialogNode NewNode(NodeType type)
    {
        if (type == NodeType.Text)
            throw new ArgumentException("Use node constructor TextNode to create nodes instead of the default one");
        return new DialogNode(type);
    }

    public static DialogNode NewSetInfoNode(IInfoValue infoValue)
    {
        Assert.IsNotNull(infoValue, nameof(infoValue));
        Assert.IsNotNull(infoValue.GetInfo(), "info");
        Assert.IsNotNull(infoValue.GetValue(), "value");

        return new DialogNode(NodeType.SetInfo)
        {
            InfoValue = infoValue
        };
    }

    public static DialogNode NewTextNode(DialogText text)
    {
        Assert.IsNotNull(text, nameof(text));
        return new DialogNode(NodeType.Text)
        {
            Text = text
        };
    }

    private DialogNode(NodeType type)
    {
        Type = type;
    }

    public override string ToString()
    {
        var prefix = base.ToString() + " - " + Type.GetText();
        return Type switch
        {
            NodeType.Text => prefix + ", Text: " + Text,
            _ => prefix
        };
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(DialogNode))
            return false;
        return Id == ((DialogNode)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}