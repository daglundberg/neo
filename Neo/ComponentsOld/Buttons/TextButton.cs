using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo
{
	class TextButton : Button
	{
		protected SpriteFont _font;
		private Color color = Color.White;
		protected Texture2D _texture;

		Vector2 textPosition;

		public Color PenColor { get; set; }

		public string Text { get; set; }

		public TextButton(Neo neo, string text, Vector2 position)
		{
			Text = text;
			if (string.IsNullOrEmpty(Text))
				Text = " ";

			_texture = neo.ButtonBg;
			_font = neo.Font;

			Bounds = new Rectangle(position.ToPoint(), _texture.Bounds.Size);

			textPosition = Neo.Center(_texture.Bounds.Size, _font.MeasureString(Text).ToPoint(), Bounds.Location);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
/*			if (_isHovering)
				color = Color.Gray;
			else
				color = Color.White;*/

			spriteBatch.Draw(_texture, Bounds, color);
			spriteBatch.DrawString(_font, Text, textPosition, color);
		}

		public override void Update(GameTime gameTime){}

		public override void Click(Point point)
		{
			throw new NotImplementedException();
		}

		public override void Refresh()
		{
			throw new NotImplementedException();
		}

	}
}
