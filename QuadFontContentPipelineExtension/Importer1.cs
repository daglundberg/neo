using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = System.String;

namespace QuadFontContentPipelineExtension
{
	[ContentImporter(".coolio", DisplayName = "Cool importer", DefaultProcessor = "Processor1")]
	public class Importer1 : ContentImporter<TImport>
	{
		public override TImport Import(string filename, ContentImporterContext context)
		{
			return System.IO.File.ReadAllText(filename);
		}
	}
}
