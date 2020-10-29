using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Neo.Components
{
	public abstract class Control : IEnumerable
	{
		public event EventHandler Clicked;
		public ScreenUnit Size { get; set; }
		public ScreenUnit PositionOffset { get; set; }
		public Flow Flow { get; set; } = Flow.TopLeft;
		public bool WantsMouse { get; set; } = false;

		public bool IsCalculated { get; protected set; }

		private List<Control> _children = new List<Control>();
		public bool HasChildren { get { return _children.Count > 0; } }
		public int ChildCount { get { return _children.Count; } }
		public Control AddChild(Control child) { _children.Add(child); return this; }
		public Control AddChildren(Control[] children) { _children.AddRange(children); return this; }

		public Control()
		{
			Size = null;
			PositionOffset = new ScreenUnit(0,0);
		}

		public Control this[int index]
		{
			get
			{
				return _children[index];
			}
		}

		public IEnumerator GetEnumerator() { return _children.GetEnumerator(); }

		//Drawable
		public Rectangle PixelBounds { get; protected set; }
		protected Vector2 _position;
		protected float _scale;

		public void Click()
		{
			Clicked?.Invoke(this, new EventArgs());
		}

		public virtual void SetPositionAndScale(Rectangle parentBounds, float scale, int numSiblings, int siblingIndex)
		{
			_scale = scale;

			if (Size == null)
				PixelBounds = parentBounds;
			else
				PixelBounds = GetPixelBoundsFromFlow(parentBounds, Flow, scale, PositionOffset, Size, numSiblings, siblingIndex);

			_position = PixelBounds.Location.ToVector2();
			IsCalculated = true;
		}

		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void Initialize(Neo neo, GraphicsDeviceManager graphics);

		protected static Rectangle GetPixelBoundsFromFlow(Rectangle parentBounds, Flow flow, float scale, ScreenUnit positionOffset, ScreenUnit size, int numSiblings, int siblingIndex)
		{
			int x = 0;
			int y = 0;
			switch (flow)
			{
				case Flow.TopLeft:
					x = parentBounds.X + (int)positionOffset.GetScaled(scale).X;
					y = parentBounds.Y + (int)positionOffset.GetScaled(scale).Y;
					break;

				case Flow.TopRight:
					x = parentBounds.X + parentBounds.Width - ((int)(size.GetScaled(scale).X + positionOffset.GetScaled(scale).X));
					y = parentBounds.Y + (int)positionOffset.GetScaled(scale).Y;
					break;

				case Flow.BottomRight:
					x = parentBounds.X + parentBounds.Width - ((int)(size.GetScaled(scale).X + positionOffset.GetScaled(scale).X));
					y = parentBounds.Y + parentBounds.Height - ((int)(size.GetScaled(scale).Y + positionOffset.GetScaled(scale).Y));
					break;

				case Flow.BottomLeft:
					x = parentBounds.X + (int)positionOffset.GetScaled(scale).X;
					y = parentBounds.Y + parentBounds.Height - ((int)(size.GetScaled(scale).Y + positionOffset.GetScaled(scale).Y));
					break;

				case Flow.Left:
					x = parentBounds.X + (int)positionOffset.GetScaled(scale).X;
					y = parentBounds.Y + (parentBounds.Height / 2 - (int)(size.GetScaled(scale).Y / 2)) + (int)positionOffset.GetScaled(scale).Y;
					break;

				case Flow.Right:
					x = parentBounds.X + parentBounds.Width - ((int)(size.GetScaled(scale).X + positionOffset.GetScaled(scale).X));
					y = parentBounds.Y + (parentBounds.Height / 2 - (int)(size.GetScaled(scale).Y / 2));
					break;

				case Flow.Center:
					x = parentBounds.X + (parentBounds.Width / 2 - (int)(size.GetScaled(scale).X / 2));
					y = parentBounds.Y + (parentBounds.Height / 2 - (int)(size.GetScaled(scale).Y / 2));
					break;

				case Flow.Top:
					x = parentBounds.X + (parentBounds.Width / 2 - (int)(size.GetScaled(scale).X / 2));
					y = parentBounds.Y + (int)positionOffset.GetScaled(scale).Y;
					break;

				case Flow.Bottom:
					x = parentBounds.X + (parentBounds.Width / 2 - (int)(size.GetScaled(scale).X / 2));
					y = parentBounds.Y + parentBounds.Height - ((int)(size.GetScaled(scale).Y + positionOffset.GetScaled(scale).Y));
					break;

				case Flow.HorizontalSharing:
					float width = (parentBounds.Width / numSiblings+1);
					int xpos = (int)(siblingIndex * width);
					x = parentBounds.X + xpos +  (int)(width / 2);
					y = parentBounds.Y + parentBounds.Height - ((int)(size.GetScaled(scale).Y + positionOffset.GetScaled(scale).Y));
					break;
			}
			return new Rectangle(new Point(x, y), size.GetScaled(scale).ToPoint());
		}
	}
}
