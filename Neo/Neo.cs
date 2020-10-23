using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neo.Components;
using System.Collections.Generic;

namespace Neo
{
    public partial class Neo
	{
		private List<Control> _controls;
		Point _screenSize;
		public Style Style { get; private set; }

		public Neo(Style style, Point screenSize)
		{
			_controls = new List<Control>();
			Style = style;
			_screenSize = screenSize;
		}

		public void AddControl(Control control){ _controls.Add(control); }

		public void Calculate(float scale)
		{
			foreach (Control control in _controls)
				ComputeControl(control, new Rectangle(0,0, _screenSize.X, _screenSize.Y), scale);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Control control in _controls)
				DrawControl(control, spriteBatch);
		}

		MouseState mouseOld;
		MouseState mouseNew;
		public void Update(GameTime gameTime)
		{
			mouseOld = mouseNew;
			mouseNew = Mouse.GetState();

			if (mouseNew.LeftButton == ButtonState.Released && mouseOld.LeftButton == ButtonState.Pressed)
				GetControlWithMouse(mouseNew.Position)?.Click();
		}

		Control moused = null;
		public Control GetControlWithMouse(Point point)
		{
			moused = null;
			foreach (Control control in _controls)
				if (control.Bounds.Contains(point))
					ControlHasMouse(control, point);

			if (moused != null)
				return moused;

			return null;
		}

		private void ComputeControl(Control control, Rectangle parentBounds, float scale)
		{
			control.Initialize(this);
			control.SetPositionAndScale(parentBounds, scale);

			foreach (Control child in control)
				ComputeControl(child, control.Bounds, scale);
		}

		private void DrawControl(Control control, SpriteBatch spriteBatch)
		{
			control.Draw(spriteBatch);

			foreach (Control child in control)
				DrawControl(child, spriteBatch);
		}

		private void ControlHasMouse(Control control, Point point)
		{
			if (control.Bounds.Contains(point))
			{
				moused = control;
				foreach (Control child in control)
					ControlHasMouse(child, point);
			}
		}
	}

	public class Style
	{
		public Texture2D
			ButtonBg,
			SwitchBg;

		public SpriteFont Font;
		public Style(ContentManager content)
		{
			ButtonBg = content.Load<Texture2D>("buttonbg");
			Font = content.Load<SpriteFont>("fonts/patuaone-big");
			SwitchBg = content.Load<Texture2D>("switch");
		}


	}

	public enum Flow
	{
		Center,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		BottomLeft,
		Left
	}
}
