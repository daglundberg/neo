using System.Globalization;

namespace Neo;

public class NeoFont
{
	public Dictionary<char, NeoGlyph> Glyphs;

	public string Name { get; private set; }
	public Texture2D Atlas { get; set; }

	public static NeoFont FromStream(Stream stream)
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

		StreamReader streamReader = new StreamReader(stream);
		List<NeoGlyph> glyphs = new List<NeoGlyph>();
		while (!streamReader.EndOfStream)
		{
			string row = streamReader.ReadLine();
			string[] cols = row.Split(',');

			char character = (char) Convert.ToInt32(cols[0]);
			float advance = (float) Convert.ToDouble(cols[1]);
			var planeBounds = new Bounds()
			{
				Left = (float) Convert.ToDouble(cols[2]),
				Bottom = (float) Convert.ToDouble(cols[3]),
				Right = (float) Convert.ToDouble(cols[4]),
				Top = (float) Convert.ToDouble(cols[5])
			};

			var atlasBounds = new Bounds()
			{
				Left = (float) Convert.ToDouble(cols[6]),
				Bottom = (float) Convert.ToDouble(cols[7]),
				Right = (float) Convert.ToDouble(cols[8]),
				Top = (float) Convert.ToDouble(cols[9])
			};

			var glyph = new NeoGlyph(character, advance, planeBounds, atlasBounds);
			glyphs.Add(glyph);
		}

		var Glyphs = new Dictionary<char, NeoGlyph>(glyphs.Count);
		foreach (NeoGlyph ng in glyphs)
			Glyphs.Add(ng.Character, ng);

		streamReader.Close();
		streamReader.Dispose();

		NeoFont f = new NeoFont();
		f.Glyphs = Glyphs;
		return f;
	}
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