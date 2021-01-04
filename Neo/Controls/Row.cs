using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo.Controls
{
	public class Row : Control
	{
		public LayoutRules LayoutRule { get; set; } = LayoutRules.LeftToRight;

		public Row() { }

		internal override void Initialize(Neo neo) { }

		internal override Block[] Blocks
		{
			get
			{
				return new Block[] { new Block { Position = Bounds.Location.ToVector2(), Size = Bounds.Size.ToVector2(), Color = new Vector4(0.1f), Radius = 10 } };
			}
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
			//A row container will arrange its children in a horizontal (or vertical) row.
			//It will ignore its childrens horizontal (or vertical) anchors			

			if (LayoutRule == LayoutRules.LeftToRight)
			{
				int x = 0;
				foreach (Control child in this)
				{
					Rectangle childBounds = CalculateChildBounds(Bounds, child, false, true);

					//Check if child is inside bounds of this row
					if ((x + child.Size.Width + child.Margins.Left + child.Margins.Right) < Bounds.Width)
					{
						child.IsClipped = false;

						childBounds.X = Bounds.X + x + child.Margins.Left;
						childBounds.Width = child.Size.Width;

						child.SetBounds(childBounds);

						x += child.Size.Width + child.Margins.Left + child.Margins.Right;
					}
					else
						child.IsClipped = true;					
				}
			}
			else if (LayoutRule == LayoutRules.RightToLeft)
			{
				int x = Bounds.Width;
				foreach (Control child in this)
				{
					Rectangle childBounds = CalculateChildBounds(Bounds, child, false, true);

					//Check if child is inside bounds of this row
					if ((x - (child.Size.Width + child.Margins.Left + child.Margins.Right)) > 0)
					{
						child.IsClipped = false;

						x -= child.Size.Width + child.Margins.Left + child.Margins.Right;
						childBounds.X = Bounds.X + x + child.Margins.Right;
						childBounds.Width = child.Size.Width;

						child.SetBounds(childBounds);
					}
					else
						child.IsClipped = true;
				}
			}
			else if (LayoutRule == LayoutRules.ShareEqually)
			{
				int x = 0;
				int w = 0;
				if (ChildCount != 0)
					w = Bounds.Width / ChildCount;

				foreach (Control child in this)
				{
					Rectangle childBounds = CalculateChildBounds(Bounds, child, false, true);

					childBounds.X = w*x + Bounds.X + child.Margins.Left;
					childBounds.Width = w - (child.Margins.Left + child.Margins.Right);

					child.SetBounds(childBounds);

					x ++;
				}
			}
			else if (LayoutRule == LayoutRules.TopToBottom)
			{
				int y = 0;
				foreach (Control child in this)
				{
					Rectangle childBounds = CalculateChildBounds(Bounds, child, true, false);

					childBounds.Y = Bounds.Y + y + child.Margins.Top;
					childBounds.Height = child.Size.Height;

					child.SetBounds(childBounds);

					y += child.Size.Height + child.Margins.Top + child.Margins.Bottom;
				}
			}
		}

		public enum LayoutRules
		{
			LeftToRight,
			RightToLeft,
			ShareEqually,
			TopToBottom,
		}
	}
}