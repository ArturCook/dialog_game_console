using System;

namespace DialogGameConsole.Enums;

public enum NodeType
{
	Undefined,
	Start,
	Text,
	Empty,
	SetInfo,
	Finish,
}

public static class NodeTypeExtensions
{
	public static string GetText(this NodeType me)
	{
		switch (me)
		{
			case NodeType.Undefined: return "Undefined";
			case NodeType.Start: return "Start";
			case NodeType.Text: return "Test";
			case NodeType.Empty: return "Empty";
			case NodeType.Finish: return "Finish";
			case NodeType.SetInfo: return "Set Info";
			default: throw new ArgumentException("Invalid Enum Value");
		}
	}
}
