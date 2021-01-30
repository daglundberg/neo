using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Neo.Controls
{
	public class Label : Control
	{
		//Control
		public string Text;
		public Color Color;
		public float FontSize = 30f;

		public Label(Neo neo, string text) : base (neo)
		{
			Color = Color.White;
			Text = text;
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}

		internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
		{
			if (_neo != null)
				guiBatch.DrawString(Text, Bounds.Location.ToVector2() + new Vector2(10, 10), FontSize, Color);			
		}
	}
}