using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Neo
{
	public abstract class Button : IGuiComponent
	{
		public Rectangle Bounds;

		protected bool _isHovering;

		public event EventHandler ClickEvent; //public bool Clicked { get; private set; }

		public Button(){}
		public Button(Rectangle bounds)
		{
			this.Bounds = bounds;
		}

		public bool HasMouseFocus(Point point)
		{
			return Bounds.Contains(point);
		}

		protected void RaiseClick()
		{
			ClickEvent?.Invoke(this, new EventArgs());
		}

		public abstract void Update(GameTime gameTime);
		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
		public abstract void Click(Point point);
		public abstract void Refresh();
	}
}
