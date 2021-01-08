using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Neo.Controls
{
	public class Label : Control
	{
		//Control
		public string Text;
		public Color Color;

		public Label(string text)
		{
			Color = Color.White;
			Text = text;
		}

		public Label(Neo neo, string text)
		{
			_neo = neo;
			Color = Color.White;
			Text = text;
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}

		internal override void Draw(GameTime gameTime, GuiBatch guiBatch)
		{
			if (_neo != null)
				guiBatch.DrawString(_neo.DefaultFont, Text, Bounds.Location.ToVector2() + new Vector2(10, 10), 30f, Color.White);
		}
	}
}
