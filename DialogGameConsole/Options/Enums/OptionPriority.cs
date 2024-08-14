using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Options.Enums;

public enum OptionPriority
{
    Undefined,
    Lowest,
    Lower,
    Low,
    Normal,
    High,
    Higher,
    Highest,
    Infinite
}

public static class OptionPriorityExtenstions
{
    public static OptionPriority Combine(this OptionPriority st1, OptionPriority st2) {
        return (OptionPriority)Math.Max((int)st1, (int)st2);
    }
}