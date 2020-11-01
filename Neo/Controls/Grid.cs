using Microsoft.Xna.Framework;

namespace Neo.Controls
{
	public class Grid : Control
	{
		public Grid()
		{

		}

		internal override void Initialize(Neo neo)
		{

		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
			//A grid container will arrange its children according to their anchors and margins.
			foreach (Control child in this)
			{
				Rectangle childBounds = new Rectangle();

				#region Horizontal position and size
				//Child is stretched horizontally (its width property will be ignored)
				if (child.Anchors.HasFlag(Anchors.Left) && child.Anchors.HasFlag(Anchors.Right))
				{
					childBounds.X = Bounds.X + child.Margins.Left;
					childBounds.Width = Bounds.Width - (child.Margins.Left + child.Margins.Right);
				}
				//Child is centered horizontally (its horizontal margin properties will be ignored)
				else if (!child.Anchors.HasFlag(Anchors.Left) && !child.Anchors.HasFlag(Anchors.Right))
				{
					childBounds.X = Bounds.X + Bounds.Width / 2 - child.Size.Width / 2;
					childBounds.Width = child.Size.Width;
				}
				//Child is anchored to the left
				else if (child.Anchors.HasFlag(Anchors.Left))
				{
					childBounds.X = Bounds.X + child.Margins.Left;
					childBounds.Width = child.Size.Width;
				}
				//Child is anchored to the right
				else if (child.Anchors.HasFlag(Anchors.Right))
				{
					childBounds.X = Bounds.X + Bounds.Width - (child.Size.Width + child.Margins.Right);
					childBounds.Width = child.Size.Width;
				}
				#endregion

				#region Vertical position and size
				//Child is stretched vertically (its height property will be ignored)
				if (child.Anchors.HasFlag(Anchors.Top) && child.Anchors.HasFlag(Anchors.Bottom))
				{
					childBounds.Y = Bounds.Y + child.Margins.Top;
					childBounds.Height = Bounds.Height - (child.Margins.Top + child.Margins.Bottom);
				}
				//Child is centered vertically (its vertical margin properties will be ignored)
				else if (!child.Anchors.HasFlag(Anchors.Top) && !child.Anchors.HasFlag(Anchors.Bottom))
				{
					childBounds.Y = Bounds.Y + Bounds.Height / 2 - child.Size.Height / 2;
					childBounds.Height = child.Size.Height;
				}
				//Child is anchored to the top
				else if (child.Anchors.HasFlag(Anchors.Top))
				{
					childBounds.Y = Bounds.Y + child.Margins.Top;
					childBounds.Height = child.Size.Height;
				}
				//Child is anchored to the bottom
				else if (child.Anchors.HasFlag(Anchors.Bottom))
				{
					childBounds.Y = Bounds.Y + Bounds.Height - (child.Size.Height + child.Margins.Bottom);
					childBounds.Height = child.Size.Height;
				}
				#endregion
				child.SetBounds(childBounds);
			}
		}
	}
}
