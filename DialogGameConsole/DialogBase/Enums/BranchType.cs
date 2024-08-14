using System;

namespace DialogGameConsole.Enums;

public enum CMD
{
	None,
	Start,
	Dialog,
	Option,
	MergeBranches,
	FinishDialog
}

public enum BranchType
{
	Undefined,
	Option_01,
	Option_02,
	Option_03,
	Timeout_01,
	Timeout_02,
	Prob_01,
	Prob_02,
	Prob_03,
	Prob_04,
}

public static class NBTypeExtensions
{
	public static bool IsOption(this BranchType me)
    {
		return me.GetOptionIndex() != -1;
	}

	public static int GetOptionIndex(this BranchType me)
    {
		return me switch
		{
			BranchType.Option_01 => 0,
			BranchType.Option_02 => 1,
			BranchType.Option_03 => 2,
			_ => -1
		};
    }

	public static bool IsTimeout(this BranchType me)
	{
		return me == BranchType.Timeout_01 ||
			me == BranchType.Timeout_02;
	}

	public static bool IsProbability(this BranchType me)
	{
		return me == BranchType.Prob_01 ||
			me == BranchType.Prob_02 || 
			me == BranchType.Prob_03 ||
			me == BranchType.Prob_04;
	}

	public static string GetText(this BranchType me)
	{
		switch (me)
		{
			case BranchType.Undefined: return "Undefined";
			case BranchType.Option_01: return "Option 01";
			case BranchType.Option_02: return "Option 02";
			case BranchType.Option_03: return "Option 03";
			case BranchType.Timeout_01: return "Timeout 01";
			case BranchType.Timeout_02: return "Timeout 02";
			case BranchType.Prob_01: return "Probability 01";
			case BranchType.Prob_02: return "Probability 02";
			case BranchType.Prob_03: return "Probability 03";
			case BranchType.Prob_04: return "Probability 04";
			default: throw new ArgumentException("Invalid Enum Value");
		}
	}
}
