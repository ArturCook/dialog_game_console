using DialogGameConsole.UI.Util;
using System;
using System.Linq;

namespace DialogGameConsole.Text.Enums;

public enum Delay
{
    Undefined = 0,
    None,
    Normal,
    Immediate,
    //Fastest,
    //Faster,
    Fast,
    Slow,
    //Slower,
    //Slowest
}

public static class DelayExtenstions
{
    public static Delay Combine(this Delay d1, Delay d2) {
        // Hidden takes priority over disabled, etc
        return (Delay)Math.Min((int)d1, (int)d2);
    }
}


