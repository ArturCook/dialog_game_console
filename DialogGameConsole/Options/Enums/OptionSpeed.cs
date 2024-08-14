using System;

namespace DialogGameConsole.Options.Enums;

public enum OptionSpeed
{
    Normal,
    Fast,
    Slow,
}

public static class OptionSpeedExtenstions
{
    public static long[] GetProfile(this OptionSpeed timing) {
        return timing switch {
            OptionSpeed.Fast => new[] { (long)1000 },
            OptionSpeed.Normal => new[] { (long)3000 },
            OptionSpeed.Slow => new[] { (long)5000 },
            _ => throw new ArgumentException($"Profile of timing {timing} is not implemented")
        };
    }

    public static OptionSpeed Combine(this OptionSpeed st1, OptionSpeed st2) {
        return (OptionSpeed)Math.Max((int)st1, (int)st2);
    }
}
