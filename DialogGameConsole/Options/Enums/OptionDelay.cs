using DialogGameConsole.UI.Util;
using System;
using System.Linq;

namespace DialogGameConsole.Options.Enums;

public enum OptionDelay
{
    Normal,
    Immediate,
    //Fastest,
    //Faster,
    Fast,
    Slow,
    //Slower,
    //Slowest
}
public static class OptionTimerExtenstions
{

    public static OptionDelay Combine(this OptionDelay st1, OptionDelay st2) {
        return (OptionDelay)Math.Max((int)st1, (int)st2);
    }
}
