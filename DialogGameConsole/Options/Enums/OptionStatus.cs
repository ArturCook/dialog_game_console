using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Options.Enums;

public enum OptionStatus
{
    Unknown,
    Empty,
    Normal,
    Loading,
    Disabled,
    Hidden,
    Obsolete
}

public static class OptionStatusExtensions
{
    public static bool IsEmpty(this OptionStatus status) => status == OptionStatus.Empty;

    public static bool IsLoading(this OptionStatus status) => status == OptionStatus.Loading;

    public static bool IsObsolete(this OptionStatus status) => status == OptionStatus.Obsolete;

    public static bool IsHidden(this OptionStatus status) => status == OptionStatus.Hidden;

    public static bool IsNormal(this OptionStatus status) => status == OptionStatus.Normal;

    public static bool IsVisible(this OptionStatus status) => !status.IsObsolete() && !status.IsHidden();

    public static OptionStatus Combine(this OptionStatus st1, OptionStatus st2) {
        return (OptionStatus)Math.Max((int)st1, (int)st2);
    }
}