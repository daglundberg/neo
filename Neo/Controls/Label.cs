using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Components
{
	public class Label : Control
	{
		//Control
		public string Text;

		public Label(string text) : base(new PixelUnit(0, 0), new PixelUnit(0, 0))
		{
			Text = text;
		}

		//Neo-init
		Neo _neo;
		public override void Initialize(Neo neo, GraphicsDeviceManager graphics)
		{
			_neo = neo;
			Size = new PixelUnit(_neo.Style.Font.MeasureString(Text).ToPoint() + new Point(20,10));
		}

		//Drawable
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(_neo.Style.Font, Text, _position, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
		}


	}
}
