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
			Size = new Size(0, 0);
		}

		internal abstract void Initialize(Neo neo);

		internal abstract void SetBounds(Rectangle bounds);

		internal Rectangle Bounds { get; set; }

		internal Block Block
		{
			get
			{
				return new Block(Bounds.Location.ToVector2(), Bounds.Size.ToVector2(), Color.White, 10);
			}
		}

		public event EventHandler Clicked;
		public void Click()
		{
			Clicked?.Invoke(this, new EventArgs());
		}
	}
}
