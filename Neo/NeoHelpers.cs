using System.Globalization;

namespace Neo;

[Flags]
public enum Anchors
{
	None = 0,
	Left = 1,
	Top = 2,
	Right = 4,
	Bottom = 8
}

public struct Margins
{
	public Margins(int margins)
	{
		Left = margins;
		Top = margins;
		Right = margins;
		Bottom = margins;
	}

	public Margins(int left, int top, int right, int bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}

	public int Left { get; set; }
	public int Top { get; set; }
	public int Right { get; set; }
	public int Bottom { get; set; }
}