using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Controls
{
	public class Row : Control
	{
		public LayoutRules LayoutRule { get; set; } = LayoutRules.LeftToRight;

		public Row()
		{

		}

		internal override void Initialize(Neo neo)
		{

		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
			//A row container will arrange its children in a horizontal row.
			//It will ignore its childrens anchors, height and vertical margins.

			int x = 0;

			if (LayoutRule == LayoutRules.LeftToRight)
				foreach (Control child in this)
				{
					child.SetBounds(new Rectangle(Bounds.X + x + child.Margins.Left, Bounds.Y, child.Size.Width, Bounds.Height));
					x += child.Size.Width + child.Margins.Left + child.Margins.Right;
				}
		}

		public enum LayoutRules
		{
			LeftToRight,
			RightToLeft,
			ShareEqually,
		}
	}
}
