using System;
using System.Linq;

namespace DialogGameConsole.UI.Util;
public class AnimationUI
{
    public const long TypingAnimationTime = 200;

    public const int TypingAnimationLength = 4;

    private static readonly string _typingDisabledText = "     ";

    //°οₒ
    private static readonly string[] _typingEnabledText = { "°οₒο", "οₒο°", "ₒο°ο", "ο°οₒ" };

    public static int GetTypingStage(long? typingTimer, int increment = 0) => typingTimer.HasValue && typingTimer.Value >= 0 ?
      (int)((typingTimer.Value / TypingAnimationTime + increment) % TypingAnimationLength) : -1;

    public static string GeTypingText(long? typingTimer, int increment = 0) {
        var typingStage = GetTypingStage(typingTimer, increment);
        if (typingStage == -1)
            return _typingDisabledText;
        return _typingEnabledText[typingStage];
    }
}
