using DialogGameConsole.DialogBase;
using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Text.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Texts;

public class DialogText : DialogBase<DialogText> {

    private PropertyWithPredicate<string> _text;

    private PropertyWithPredicate<Delay> _delay;

    public readonly Character Character;

    public string GetText(InfoMap map) => _text.Get(map);

    public IEnumerable<(long, bool)> GetDelayProfile(InfoMap map) {
        var wordCount = (int)Math.Max(2, Math.Ceiling(GetText(map).Length / 5.0));

        var delay = _delay.Get(map);
        if(delay == Delay.Immediate) {
            yield return (0, false);
            yield break;
        }
        
        yield return (GetStartTime(delay, wordCount), false);

        var remainingTime = GetTypingTime(delay, wordCount);
        while(remainingTime > 0) {

            var timeTyping = GetConsecutiveTimeTyping(delay, remainingTime);
            yield return (timeTyping, true);
            remainingTime -= timeTyping;

            if (remainingTime <= 0)
                yield break;

            var timeStopped = GetStopTypingTime(delay, remainingTime);
            yield return (timeStopped, false);
        }
    }

    private long GetConsecutiveTimeTyping(Delay timing, long remainingTime) {
        long time = timing switch {
            Delay.Immediate => throw new ArgumentException($"Profile of timing {timing} is not implemented"),
            Delay.Fast => 4000,
            Delay.Normal => 3000,
            Delay.Slow => 1500,
            _ => throw new ArgumentException($"Profile of timing {timing} is not implemented")
        };
        time = AddRandomness(time, 0);
        return Math.Min(time, remainingTime);
    }

    private long GetStopTypingTime(Delay timing, long remainingTime) {
        long time = timing switch {
            Delay.Immediate => throw new ArgumentException($"Profile of timing {timing} is not implemented"),
            Delay.Fast => 300,
            Delay.Normal => 800,
            Delay.Slow => 1500,
            _ => throw new ArgumentException($"Profile of timing {timing} is not implemented")
        };
        time = AddRandomness(time, 0);
        return time;
    }

    private long GetTypingTime(Delay delay, int wordCount) {
        var time = (long)(GetTypingModifier(delay) * GetTypingTime(wordCount));
        time = AddRandomness(time, 0);
        return time;
    }

    public double GetTypingTime(int wordCount) {
        if (wordCount <= 2)
            return wordCount * 60 * 1000 / 80;
        if (wordCount <= 6)
            return wordCount * 60 * 1000 / 90;
        if (wordCount <= 20)
            return wordCount * 60 * 1000 / 110;
        return wordCount * 60 * 1000 / 130;
    }

    public static double GetTypingModifier(Delay timing) {
        return timing switch {
            Delay.Immediate => throw new ArgumentException($"Profile of timing {timing} is not implemented"),
            Delay.Fast => 0.6,
            Delay.Normal => 1,
            Delay.Slow => 1.5,
            _ => throw new ArgumentException($"Profile of timing {timing} is not implemented")
        };
    }

    private long GetStartTime(Delay delay, int wordCount) {
        var time = (long)(GetInitialDelayModifier(delay) * GetInitialDelayTime(wordCount));
        time = AddRandomness(time, 0);
        return time;
    }

    public double GetInitialDelayTime(int wordCount) {
        if (wordCount <= 2) return 1200;
        if (wordCount <= 6) return 1400;
        if (wordCount <= 20) return 2000;
        return 2100; 
    }

    public static double GetInitialDelayModifier(Delay timing) {
        return timing switch {
            Delay.Immediate => throw new ArgumentException($"Profile of timing {timing} is not implemented"),
            Delay.Fast => 0.6,
            Delay.Normal => 1,
            Delay.Slow => 1.5,
            _ => throw new ArgumentException($"Profile of timing {timing} is not implemented")
        };
    }

    private static long AddRandomness(long x, double p) {
        return (long)(x * (1 + p * (Game.UIRandom.NextDouble() - 0.5) / 0.5));
    }

    public void AddText(string text, IInfoPredicate predicate) => _text.AddVariation(text, predicate);

    public void AddDelay(Delay delay, IInfoPredicate predicate) => _delay.AddVariation(delay, predicate);

    internal DialogText(Character character, string baseText, Delay baseDelay = Delay.Normal) {
        Assert.IsNotDefault(character, nameof(character));
        Assert.IsNotEmpty(baseText, nameof(baseText));

        Character = character;
        _text = new(baseText, (_, v2) => v2);
        _delay = new(baseDelay, (s1, s2) => s1.Combine(s2));
    }

    public override string ToString() {
        return _text.ToString() + "\n" + _delay.ToString();
    }
}
