using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Neo
{
	public class NeoFont
	{
		public string Name { get; private set; }
		public Dictionary<char, NeoGlyph> Glyphs;
		public Texture2D Atlas { get; set; }

		public NeoFont()
		{

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

		public char Character { get; private set; }
		public float Advance { get; private set; }
		public Bounds AtlasBounds { get; private set; }
		public Bounds PlaneBounds { get; private set; }

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

		public static Bounds operator * (Bounds a, float b) =>
			new Bounds(
				a.Left * b,
				a.Bottom * b,
				a.Right * b,
				a.Top * b);
	}


}
