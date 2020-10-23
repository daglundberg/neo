using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Components
{
	public class Button : Control
	{
		//Control
		public string Text;
		public Button(string text) : base(new PixelUnit(0, 0), new PixelUnit(0, 0))
		{
			Text = text;
		}

		//Neo-init
		Neo _neo;
		public override void Initialize(Neo neo)
		{
			_neo = neo;
			Size = new PixelUnit(_neo.Style.Font.MeasureString(Text).ToPoint());
			//This line is mixing Pixel Units with pixels.... fix it!
		}

		//Drawable
		public override void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(_neo.Style.ButtonBg, _position, null, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(_neo.Style.Font, Text, _position, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
		}


	}
}
