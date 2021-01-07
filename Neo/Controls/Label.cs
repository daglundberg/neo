using Microsoft.Xna.Framework;
using Neo.Extensions;
using System;

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

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}
	}
}
