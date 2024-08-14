using System;

namespace DialogGameConsole.Text.Enums;

public enum Character
{
    Undefined,
    Player,
    Npc,
}

public static class MessageSourceExtensions
{
    public static string GetText(this Character me) {
        switch (me) {
            case Character.Undefined:
                return "Undefined";
            case Character.Player:
                return "Player";
            case Character.Npc:
                return "NPC";
            default:
                throw new ArgumentException("Invalid Enum Value");
        }
    }
}
