using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neo.Controls
{
	public class Switch : Control
	{
		public Switch()
		{
			Clicked += Switch_Clicked;
		}

		public bool Checked = false;

		internal override void Initialize(Neo neo)
		{

		}

		private void Switch_Clicked(object sender, EventArgs e)
		{
			Size = new Size(100, 50);
			Checked = !Checked;
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
		}
	}
}
