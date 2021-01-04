using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo.Controls
{
	public class Button : Control
	{
		//Control
		public string Text;
		public Color Color { get; set; } = new Color(0.9f, 0.3f, 0.0f, 1f);

		public Button() : base (false)
		{
			Text = " ";
		}

		public Button(string text) : base(false)
		{
			Text = text;
		}

		internal override void Initialize(Neo neo) { }

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}

		internal override void Draw(GameTime gameTime, GuiBatch guiBatch)
		{
			if (IsClipped != true)
			{
				Vector4 c = new Vector4(0.9f, 0.3f, (float)(Math.Abs(Math.Sin(2 * gameTime.TotalGameTime.TotalSeconds))), 1f);
				guiBatch.DrawBlock(new Block { Position = Bounds.Location.ToVector2(), Size = Bounds.Size.ToVector2(), Color = c, Radius = 8 });
			}
		}
	}
}
