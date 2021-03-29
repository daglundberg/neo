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
		public bool WantsMouse = false;
		protected Neo _neo;

		internal bool IsClipped { get; set; }

		#region Children
		private List<Control> _children;
		public bool HasChildren { get { return _children.Count > 0; } }
		public int ChildCount { get { return _children.Count; } }
		public Control AddChild(Control child) { _children.Add(child); SetBounds(Bounds); return this; }
		public Control AddChildren(Control[] children) { _children.AddRange(children); SetBounds(Bounds); return this; }
		public void RemoveChild(Control child) { _children.Remove(child); }
		public IEnumerator GetEnumerator() { return _children.GetEnumerator(); }
		public Control this[int index] { get { return _children[index]; } }
		#endregion

		public Control(Neo neo) : this(neo, false){}
		public Control(Neo neo, bool CanHaveChildren)
		{
			_neo = neo;

			if (CanHaveChildren)
				_children = new List<Control>();
			else
				_children = new List<Control>(0);

			Size = new Size(10, 10);
		}

		internal abstract void SetBounds(Rectangle bounds);

		internal Rectangle Bounds { get; set; }

		internal virtual void Draw(GameTime gameTime, NeoBatch guiBatch)
		{
			if (IsClipped != true)
			{
				foreach (Control c in this)
					c.Draw(gameTime, guiBatch);
			}
		}

		public bool ListensForMouseOrTouchAt(Point mousePosition)
		{
			foreach (Control child in this)
				if (child.ListensForMouseOrTouchAt(mousePosition))
					return true;

			if (IsClipped)
				return false;

			return WantsMouse;
		}

		//Returns true if the click was consumed
		public bool Click(Point mousePosition)
		{
			foreach (Control child in this)
				if (child.ListensForMouseOrTouchAt(mousePosition))				
					if (child.Click(mousePosition))
						return true;					
				

			if (Bounds.Contains(mousePosition) && WantsMouse)
			{
				Clicked?.Invoke(this, new EventArgs());
				return true;
			}
			else
				return false;
		}

		public event EventHandler Clicked;


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