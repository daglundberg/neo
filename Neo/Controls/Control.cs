using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Neo.Components
{
	public abstract class Control : IEnumerable
	{
		public EventHandler Clicked;
		public PixelUnit Size { get; set; }
		public PixelUnit Position { get; set; }
		public Flow Flow { get; set; }

		private List<Control> _children;
		public bool HasChildren { get { return _children.Count > 0; } }
		public void AddChild(Control control) { _children.Add(control); }

		public Control() : this(new PixelUnit(0, 0), new PixelUnit(10, 10))
		{

		}

		public Control(PixelUnit position, PixelUnit size)
		{
			_children = new List<Control>();
			Size = size;
			Position = position;
			Flow = Flow.TopLeft;
		}

		public IEnumerator GetEnumerator(){ return _children.GetEnumerator(); }

		//Drawable
		public Rectangle Bounds { get; protected set; }
		protected Vector2 _position;
		protected float _scale;

		public void Click()
		{
			Clicked?.Invoke(this, new EventArgs());
		}

		public void SetPositionAndScale(Rectangle parentBounds, float scale)
		{
			_scale = scale;

			int x = 0;
			int y = 0;

			switch (this.Flow)
			{
				case Flow.TopLeft:
					x = parentBounds.X + (int)Position.GetScaled(scale).X;
					y = parentBounds.Y + (int)Position.GetScaled(scale).Y;
					break;

				case Flow.TopRight:
					x = parentBounds.X + parentBounds.Width - ((int)(Size.GetScaled(scale).X + Position.GetScaled(scale).X));
					y = parentBounds.Y + (int)Position.GetScaled(scale).Y;
					break;

				case Flow.BottomRight:
					x = parentBounds.X + parentBounds.Width - ((int)(Size.GetScaled(scale).X + Position.GetScaled(scale).X));
					y = parentBounds.Y + parentBounds.Height - ((int)(Size.GetScaled(scale).Y + Position.GetScaled(scale).Y));
					break;

				case Flow.BottomLeft:
					x = parentBounds.X + (int)Position.GetScaled(scale).X;
					y = parentBounds.Y + parentBounds.Height - ((int)(Size.GetScaled(scale).Y + Position.GetScaled(scale).Y));
					break;

				case Flow.Left:
					x = parentBounds.X + (int)Position.GetScaled(scale).X;
					y = parentBounds.Y + (parentBounds.Height/2 - (int)(Size.GetScaled(scale).Y/2));
					break;

				case Flow.Right:
					x = parentBounds.X + parentBounds.Width - ((int)(Size.GetScaled(scale).X + Position.GetScaled(scale).X));
					y = parentBounds.Y + (parentBounds.Height / 2 - (int)(Size.GetScaled(scale).Y / 2));
					break;

				case Flow.Center:
					x = parentBounds.X + (parentBounds.Width / 2 - (int)(Size.GetScaled(scale).X / 2));
					y = parentBounds.Y + (parentBounds.Height / 2 - (int)(Size.GetScaled(scale).Y / 2));
					break;

				case Flow.Top:
					x = parentBounds.X + (parentBounds.Width / 2 - (int)(Size.GetScaled(scale).X / 2));
					y = parentBounds.Y + (int)Position.GetScaled(scale).Y;
					break;

				case Flow.Bottom:
					x = parentBounds.X + (parentBounds.Width / 2 - (int)(Size.GetScaled(scale).X / 2));
					y = parentBounds.Y + parentBounds.Height - ((int)(Size.GetScaled(scale).Y + Position.GetScaled(scale).Y));
					break;
			}

			Bounds = new Rectangle(new Point(x, y), Size.GetScaled(scale).ToPoint());
			_position = Bounds.Location.ToVector2();
		}

		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void Initialize(Neo neo);
	}
}
