using Microsoft.Xna.Framework.Content.Pipeline;
using Neo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using MonoGame.Framework.Content.Pipeline.Builder;

namespace NeoFontContentPipelineExtension;

[ContentImporter(".neofont", DisplayName = "NeoFont glyph data importer", DefaultProcessor = "PassThroughProcessor")]
public class NeoFontImporter : ContentImporter<NeoFont>
{
	public override NeoFont Import(string filename, ContentImporterContext context)
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		
		StreamReader streamReader = new StreamReader(filename);
		List<NeoGlyph> glyphs = new List<NeoGlyph>();
		while (!streamReader.EndOfStream)
		{
			string row = streamReader.ReadLine();
			string[] cols = row.Split(',');

			char character = (char)Convert.ToInt32(cols[0]);
			float advance = (float)Convert.ToDouble(cols[1]);
			var planeBounds = new Bounds()
			{
				Left = (float)Convert.ToDouble(cols[2]),
				Bottom = (float)Convert.ToDouble(cols[3]),
				Right = (float)Convert.ToDouble(cols[4]),
				Top = (float)Convert.ToDouble(cols[5])
			};
			Console.Write(planeBounds.Top);
			Console.Write(", ");
			var atlasBounds = new Bounds()
			{
				Left = (float)Convert.ToDouble(cols[6]),
				Bottom = (float)Convert.ToDouble(cols[7]),
				Right = (float)Convert.ToDouble(cols[8]),
				Top = (float)Convert.ToDouble(cols[9])
			};
			Console.Write("\n");
			Console.Write(atlasBounds.Top);
			Console.Write(", ");
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