namespace Neo;

public class NeoFont
{
	public Dictionary<char, NeoGlyph> Glyphs;

	public string Name { get; private set; }
	public Texture2D Atlas { get; set; }
}

public class NeoGlyph
{
	public NeoGlyph()
	{
	}

	public NeoGlyph(char character, float advance, Bounds planeBounds, Bounds atlasBounds)
	{
		Character = character;
		Advance = advance;
		PlaneBounds = planeBounds;
		AtlasBounds = atlasBounds;
	}

	public char Character { get; }
	public float Advance { get; }
	public Bounds AtlasBounds { get; }
	public Bounds PlaneBounds { get; }
}

public struct Bounds
{
	public Bounds(float left, float bottom, float right, float top)
	{
		Left = left;
		Bottom = bottom;
		Right = right;
		Top = top;
	}

	public float Left;
	public float Bottom;
	public float Right;
	public float Top;

	public static Bounds operator *(Bounds a, float b)
	{
		return new Bounds(
			a.Left * b,
			a.Bottom * b,
			a.Right * b,
			a.Top * b);
	}
}