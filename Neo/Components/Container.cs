using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;

namespace Neo.Components
{
	public abstract class Control : IEnumerable
	{
		public PixelUnit Size { get; set; }
		public PixelUnit Position { get; set; }
		public Flow Flow { get; set; }

		public bool HasChildren	{ get { return _children.Count > 0;} }

		private List<Control> _children;

		public Control(PixelUnit position, PixelUnit size)
		{
			_children = new List<Control>();
			Size = size;
			Position = position;
			Flow = Flow.TopLeft;
		}

		public void AddChild(Control control){ _children.Add(control); }

		public abstract void Initialize(Neo neo);

		public IEnumerator GetEnumerator(){	return _children.GetEnumerator(); }

		//Drawable
		public Rectangle Bounds { get; protected set; }
		protected Vector2 _position;
		protected float _scale;

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
	}

	public class Button : Control
	{
		//Control
		public string Text;
		public Button(string text) : base(new PixelUnit(0, 0), new PixelUnit(0, 0))
		{
			Text = text;
		}

		//Neo-init
		Neo _neo;
		public override void Initialize(Neo neo)
		{
			_neo = neo;
			Size = new PixelUnit(_neo.Style.Font.MeasureString(Text).ToPoint()); 
			//This line is mixing Pixel Units with pixels.... fix it!
		}

		//Drawable
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_neo.Style.ButtonBg, _position, null, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(_neo.Style.Font, Text, _position, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
		}
	}

	public class Container : Control
	{
		public Container(PixelUnit position, PixelUnit size) : base(position, size)	{}
		public override void Initialize(Neo neo){}
		public override void Draw(SpriteBatch spriteBatch){}
	}

	public struct PixelUnit
	{
		public PixelUnit(int width, int height) { Width = width; Height = height; }
		public PixelUnit(Point point) { Width = point.X; Height = point.Y; }
		private int Width;
		private int Height;

		public Vector2 GetScaled(float scale)
		{
			return new Vector2(Width * scale, Height * scale);
		}
	}
}
