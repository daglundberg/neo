using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Components
{
	public class Button : Control
	{
		//Control
		public string Text;

		public Button(string text)
		{
			Text = text;
			WantsMouse = true;
		}

		//Neo-init
		Neo _neo;
		public override void Initialize(Neo neo, GraphicsDeviceManager graphics)
		{
			_neo = neo;
			Size = new PixelUnit(_neo.Style.Font.MeasureString(Text).ToPoint() + new Point(20,10));
			//This line is mixing Pixel Units with pixels.... fix it!

			Point size = new Point((int)Size.GetScaled(_neo.Scale).X, (int)Size.GetScaled(_neo.Scale).Y);

			_bg = new Texture2D(graphics.GraphicsDevice, size.X, size.Y);
			Color[] data = new Color[size.X * size.Y];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Gray;
			_bg.SetData(data);
		}

		//Drawable
		private Texture2D _bg;
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_bg, _position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(_neo.Style.Font, Text, _position, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
		}


	}
}
