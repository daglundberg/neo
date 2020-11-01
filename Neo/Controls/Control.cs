using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Neo.Controls
{
	public abstract class Control : IEnumerable
	{
		public Size Size { get; set; }
		public Margins Margins { get; set; }
		public Anchors Anchors { get; set; }

		internal bool HasChanged { get; set; }

		#region Children
		private List<Control> _children = new List<Control>();
		public bool HasChildren { get { return _children.Count > 0; } }
		public int ChildCount { get { return _children.Count; } }
		public Control AddChild(Control child) { _children.Add(child); return this; }
		public Control AddChildren(Control[] children) { _children.AddRange(children); return this; }
		public void RemoveChild(Control child) { _children.Remove(child); }
		public IEnumerator GetEnumerator() { return _children.GetEnumerator(); }
		public Control this[int index] { get { return _children[index]; } }
		#endregion
		public Control()
		{
			Size = new Size(10, 10);
		}

		internal abstract void Initialize(Neo neo);

		internal abstract void SetBounds(Rectangle bounds);

		internal Rectangle Bounds { get; set; }

		internal Block Block
		{
			get
			{
				return new Block() { Position = Bounds.Location.ToVector2(), Size = Bounds.Size.ToVector2(), Color = new Color(0.3f, 0.2f, 0.5f, 0.3f).ToVector4(), Radius = 10 };
			}
		}

		public event EventHandler Clicked;
		public void Click()
		{
			Clicked?.Invoke(this, new EventArgs());
		}

		internal Rectangle CalculateChildBounds(Rectangle parentBounds, Control child, bool calculateHorizontal, bool calculateVertical)
		{
			Rectangle childBounds = new Rectangle();

			if (calculateHorizontal)
			{
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
			}

			if (calculateVertical)
			{
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
			}

			return childBounds;
		}

	}
}
