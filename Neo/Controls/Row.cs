﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

		internal override Block Block
		{
			get
			{
				return new Block() { Position = Bounds.Location.ToVector2(), Size = Bounds.Size.ToVector2(), Color = new Color(0.1f, 0.1f, 0.1f, 0.01f).ToVector4(), Radius = 10 };
			}
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
			//A row container will arrange its children in a horizontal row.
			//It will ignore its childrens horizontal anchors

			if (LayoutRule == LayoutRules.LeftToRight)
			{
				int x = 0;
				foreach (Control child in this)
				{
					Rectangle childBounds = CalculateChildBounds(Bounds, child, false, true);

					childBounds.X = Bounds.X + x + child.Margins.Left;
					childBounds.Width = child.Size.Width;

					child.SetBounds(childBounds);

					x += child.Size.Width + child.Margins.Left + child.Margins.Right;
				}
			}
			else if (LayoutRule == LayoutRules.RightToLeft)
			{
				int x = Bounds.Width;
				foreach (Control child in this)
				{
					Rectangle childBounds = CalculateChildBounds(Bounds, child, false, true);
					x -= child.Size.Width + child.Margins.Left + child.Margins.Right;
					childBounds.X = Bounds.X + x + child.Margins.Right;
					childBounds.Width = child.Size.Width;

					child.SetBounds(childBounds);


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
