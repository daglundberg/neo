using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TInput = System.String;
/*using TOutput = QuadFontContentPipelineExtension.Coolio;
using TWrite = QuadFontContentPipelineExtension.Coolio;
using TRead = QuadFontContentPipelineExtension.Coolio;*/
using Microsoft.Xna.Framework.Content;

namespace QuadFontContentPipelineExtension
{
/*    public class Coolio
	{
        public string Text;
        public Coolio(string text)
		{
            Text = text;
		}
	}

	[ContentProcessor(DisplayName = "Processor1")]
	class Processor1 : ContentProcessor<TInput, TOutput>
	{
		public override TOutput Process(TInput input, ContentProcessorContext context)
		{
            return new Coolio(input + "JIIIHAA222");
		}
	}

    [ContentTypeWriter]
    public class LevelWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.Text);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "QuadFontContentPipelineExtension.LevelReader, QuadFontContentPipelineExtension";
        }
    }

    public class LevelReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            return new Coolio(input.ReadString());
        }
    }*/
}
