using Microsoft.Xna.Framework;
using System;

namespace Neo.Controls
{	
	public class Grid : Control
	{
		public Grid(Neo neo) : base(neo, true) { }

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
			//A grid container will arrange its children according to their anchors and margins.
			foreach (Control child in this)
			{
				child.SetBounds(CalculateChildBounds(Bounds, child, true, true) );
			}
		}
	}
}
