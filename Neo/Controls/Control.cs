using Microsoft.Xna.Framework.Input;

namespace Neo.Controls;

public abstract class Control : IEnumerable
{
	protected Neo _neo;
	public bool WantsMouse = false;

	public Control(Neo neo) : this(neo, false)
	{
	}

	public Control(Neo neo, bool CanHaveChildren)
	{
		_neo = neo;

		if (CanHaveChildren)
			_children = new List<Control>();
		else
			_children = new List<Control>(0);

		Size = new Size(10, 10);
	}

	public Size Size { get; set; }
	public Margins Margins { get; set; }
	public Anchors Anchors { get; set; }

	public bool HasFocus { get; }
	
	internal bool IsClipped { get; set; }

	internal Rectangle Bounds { get; set; }

	internal abstract void SetBounds(Rectangle bounds);

	internal virtual void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (IsClipped != true)
			foreach (Control c in this)
				c.Draw(gameTime, guiBatch);
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

		return false;
	}

	//Returns true if the event was consumed
	public bool MouseDown(Point mousePosition)
	{
		foreach (Control child in this)
			if (child.ListensForMouseOrTouchAt(mousePosition))
				if (child.MouseDown(mousePosition))
					return true;


		if (Bounds.Contains(mousePosition) && WantsMouse)
		{
			MouseDowned?.Invoke(this, new EventArgs());
			return true;
		}

		return false;
	}

	public event EventHandler Clicked;
	public event EventHandler MouseDowned;

	public virtual void KeyPress(Keys[] key)
	{
		
	}


	internal Rectangle CalculateChildBounds(Rectangle parentBounds, Control child, bool calculateHorizontal,
		bool calculateVertical)
	{
		var childBounds = new Rectangle();

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

	#region Children

	private readonly List<Control> _children;
	public bool HasChildren => _children.Count > 0;
	public int ChildCount => _children.Count;

	public Control AddChild(Control child)
	{
		_children.Add(child);
		SetBounds(Bounds);
		return this;
	}

	public Control AddChildren(Control[] children)
	{
		_children.AddRange(children);
		SetBounds(Bounds);
		return this;
	}

	public void RemoveChild(Control child)
	{
		//TODO:This is bad
		_children.Remove(child);
	}

	public IEnumerator GetEnumerator()
	{
		return _children.GetEnumerator();
	}

	public Control this[int index] => _children[index];

	#endregion
}