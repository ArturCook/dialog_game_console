using DialogGameConsole.Options.Enums;
using DialogGameConsole.UI.Util;
using DialogGameConsole.Util;
using Spectre.Console;

namespace DialogGameConsole.UI.Entities;

public class DialogOptionUI
{
    public string Text { get; private set; } = "";

    private OptionStatus Status { get; set; } = OptionStatus.Empty;

    public bool IsLoading() => Status == OptionStatus.Loading;

    public bool IsDisabled() => Status == OptionStatus.Disabled;

    public bool IsEmpty() => Status == OptionStatus.Empty;

    public bool IsSelectable() => Status == OptionStatus.Normal;

    public static DialogOptionUI New()
    {
        return new DialogOptionUI();
    }

    public void SetText(string text)
    {
        Assert.IsNotEqual(Status, OptionStatus.Empty, nameof(Status));
        Assert.IsNotEmpty(text, nameof(text));
        Text = text;
    }

    public void SetStatus(OptionStatus status)
    {
        if (status is not OptionStatus.Empty or OptionStatus.Normal or OptionStatus.Loading)
        Assert.IsNotDefault(status, nameof(status));
        Status = status;
        Text = "";
    }


}
