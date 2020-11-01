using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo
{
	public class ActionButton : Button
	{
		private double animationTime = 0;
		protected SpriteFont _font;
		Vector2 _textPosition;
		Vector2 _glowPosition;
		protected Texture2D _texture;
		protected Texture2D _glowTexture;
		protected float glow;

		public Color PenColor { get; set; }

		public string Text { get; set; }

		public ActionButton(Neo neo, string text, Vector2 position, bool primary)
		{
			Text = text;
			
			if (string.IsNullOrEmpty(Text))
				Text = " ";

			_texture = neo.ActionButtonBg;
			_glowTexture = null;

			Bounds = new Rectangle(position.ToPoint(), _texture.Bounds.Size);

			PenColor = Color.White;
			glow = 0.0f;

			_font = neo.Font;

			_textPosition = Neo.Center(_texture.Bounds.Size, _font.MeasureString(Text).ToPoint(), Bounds.Location);
			_glowPosition = Neo.Center(_texture.Bounds.Size, _glowTexture.Bounds.Size, Bounds.Location);
		}

		public override void Refresh()
		{
			_textPosition = Neo.Center(_texture.Bounds.Size, _font.MeasureString(Text).ToPoint(), Bounds.Location);
			_glowPosition = Neo.Center(_texture.Bounds.Size, _glowTexture.Bounds.Size, Bounds.Location);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			animationTime += 0.002f * gameTime.ElapsedGameTime.TotalMilliseconds;
			glow = (float)(Math.Sin(animationTime) * 0.3 + 0.6);

			if (animationTime < 1f)
			{
				spriteBatch.Draw(_glowTexture, _glowPosition, Color.White * glow * (float)animationTime);
				spriteBatch.Draw(_texture, Bounds, Color.White * (float)animationTime);

				spriteBatch.DrawString(_font, Text, _textPosition, PenColor * glow* (float)animationTime);
			}
			else
			{
				spriteBatch.Draw(_glowTexture, _glowPosition, Color.White * glow);
				spriteBatch.Draw(_texture, Bounds, Color.White);

				spriteBatch.DrawString(_font, Text, _textPosition, PenColor * glow);
			}
		}

		public override void Update(GameTime gameTime){}

		public override void Click(Point point)
		{
			if (base.HasMouseFocus(point))
				RaiseClick();
			animationTime = 0;
		}

	}
}
