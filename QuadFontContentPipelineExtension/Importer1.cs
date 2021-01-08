using Microsoft.Xna.Framework.Content.Pipeline;
using Neo;
using System;
using System.Collections.Generic;
using System.IO;
using TImport = Neo.NeoFont;

namespace QuadFontContentPipelineExtension
{
	[ContentImporter(".neofont", DisplayName = "Neo font data importer", DefaultProcessor = "")]
	public class Importer1 : ContentImporter<TImport>
	{
		public override TImport Import(string filename, ContentImporterContext context)
		{

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
				var atlasBounds = new Bounds()
				{
					Left = (float)Convert.ToDouble(cols[6]),
					Bottom = (float)Convert.ToDouble(cols[7]),
					Right = (float)Convert.ToDouble(cols[8]),
					Top = (float)Convert.ToDouble(cols[9])
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
}
