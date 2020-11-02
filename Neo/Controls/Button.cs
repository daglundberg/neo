using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo.Controls
{
	public class Button : Control
	{
		//Control
		public string Text;

		public Button() : base (false)
		{
			Text = " ";
		}

		public Button(string text) : base(false)
		{
			Text = text;
		}


		internal override void Initialize(Neo neo)
		{

		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}

		internal override Block Block
		{
			get
			{
				return new Block() { Position = Bounds.Location.ToVector2(), Size = Bounds.Size.ToVector2(), Color = new Color(0.9f, 0.3f, 0.0f, 1f).ToVector4(), Radius = 14 };
			}
		}
	}


}
