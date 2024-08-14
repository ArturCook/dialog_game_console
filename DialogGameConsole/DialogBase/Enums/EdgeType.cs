using System;

namespace DialogGameConsole.Enums;

public enum EdgeType
{
	Undefined,
	Direct,
	Stop,
	Timing,
	Option,
	Probability,
}

public static class EdgeTypeExtensions
{
    public static string GetText(this EdgeType me)
    {
        switch (me)
        {
			case EdgeType.Undefined: return "Undefined";
			case EdgeType.Direct: return "Direct";
			case EdgeType.Timing: return "Timing";
			case EdgeType.Option: return "Option";
			case EdgeType.Probability: return "Probability";
			default: throw new ArgumentException("Invalid Enum Value");
		}
    }
}

