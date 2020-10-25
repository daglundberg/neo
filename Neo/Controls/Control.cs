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
		public PixelUnit Size { get; set; }
		public PixelUnit PositionOffset { get; set; }
		public Flow Flow { get; set; } = Flow.TopLeft;
		public bool WantsMouse { get; set; } = false;

		public bool IsCalculated { get; protected set; }

		private List<Control> _children = new List<Control>();
		public bool HasChildren { get { return _children.Count > 0; } }
		public Control AddChild(Control child) { _children.Add(child); return this; }
		public Control AddChildren(Control[] children) { _children.AddRange(children); return this; }

		public Control()
		{
			Size = null;
			PositionOffset = new PixelUnit(0,0);
		}

		public IEnumerator GetEnumerator() { return _children.GetEnumerator(); }

		//Drawable
		public Rectangle Bounds { get; protected set; }
		protected Vector2 _position;
		protected float _scale;

		public void Click()
		{
			Clicked?.Invoke(this, new EventArgs());
		}

		public virtual void SetPositionAndScale(Rectangle parentBounds, float scale)
		{
			_scale = scale;

			if (Size == null)
				Bounds = parentBounds;
			else
				Bounds = GetPixelBoundsFromFlow(parentBounds, Flow, scale, PositionOffset, Size);

			_position = Bounds.Location.ToVector2();
			IsCalculated = true;
		}

		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void Initialize(Neo neo, GraphicsDeviceManager graphics);

		protected static Rectangle GetPixelBoundsFromFlow(Rectangle parentBounds, Flow flow, float scale, PixelUnit positionOffset, PixelUnit size)
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
			}
			return new Rectangle(new Point(x, y), size.GetScaled(scale).ToPoint());
		}
	}
}
