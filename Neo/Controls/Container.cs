using Microsoft.Xna.Framework.Graphics;

namespace Neo.Components
{
	public class Container : Control
	{
		public Container(PixelUnit position, PixelUnit size) : base(position, size) { }
		public override void Initialize(Neo neo) { }
		public override void Draw(SpriteBatch spriteBatch) { }
	}
}
