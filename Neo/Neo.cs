using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Framework.Utilities;
using Neo.Components;
using System;
using System.Collections.Generic;

namespace Neo
{
    public partial class Neo
	{
		private List<Control> _controls;
		Point _screenSize;
		public Style Style { get; private set; }
		private MonoGamePlatform _currentPlatform;
		public float Scale { get; private set; }
		private float _scaleMultiplier = 1;

		public float ScaleMultiplier
		{
			get
			{
				return _scaleMultiplier;
			}

			set
			{
				_scaleMultiplier = value;
				Calculate(true);
			}
		}

		private GraphicsDeviceManager _graphics;

		public Neo(GraphicsDeviceManager graphics, Style style, MonoGamePlatform platform)
		{
			_graphics = graphics;
			_currentPlatform = platform;
			_controls = new List<Control>();
			Style = style;
		}

		public void AddControl(Control control){ _controls.Add(control); Calculate(false); }
		public void AddControls(Control[] controls){ _controls.AddRange(controls); Calculate(false); }
		public void RemoveControl(Control control) { _controls.Remove(control); Calculate(false); }

		public void Calculate(bool force)
		{
			CalculateScale();
			foreach (Control control in _controls)
				if (!control.IsCalculated || force)
					ComputeControl(control, new Rectangle(0, 0, _screenSize.X, _screenSize.Y));
		}

		private void CalculateScale()
		{
			_screenSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
			Scale = ((float)_screenSize.X / 2327f + (float)_screenSize.Y / 1309f) / 2;
			Scale *= ScaleMultiplier;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Control control in _controls)
				DrawControl(control, spriteBatch);
		}

		public void Update(GameTime gameTime)
		{
			if (_currentPlatform == MonoGamePlatform.DesktopGL)
				CheckMouse();
			else if (_currentPlatform == MonoGamePlatform.Android)
				CheckTouch();
		}

		MouseState _mouseStateOld, _mouseStateNew;
		private void CheckMouse()
		{
			_mouseStateOld = _mouseStateNew;
			_mouseStateNew = Mouse.GetState();

			//Check if a mouse click occured
			if (_mouseStateOld.LeftButton == ButtonState.Pressed && _mouseStateNew.LeftButton == ButtonState.Released)
			{
				Control c = GetControlWithMouse(_mouseStateNew.Position);
				if (c != null)
					if (c.WantsMouse)
						c.Click();

				return;
			}
		}

		private void CheckTouch()
		{
			TouchCollection tc = TouchPanel.GetState();
			foreach (TouchLocation tl in tc)
			{
				if (tl.State == TouchLocationState.Pressed)
				{
					Point pos = tl.Position.ToPoint();
					Control c = GetControlWithMouse(pos);
					if (c != null)
						if (c.WantsMouse)
							c.Click();
				}
			}
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

		public bool WantsMouse(Point point)
		{
			GetControlWithMouse(point);
			if (moused == null)
				return false;

			return moused.WantsMouse;
		}

		public void Click(Point point)
		{
			moused.Click();
		}

		private void ComputeControl(Control control, Rectangle parentBounds)
		{
			control.Initialize(this, _graphics);
			control.SetPositionAndScale(parentBounds, Scale);

			foreach (Control child in control)
				ComputeControl(child, control.Bounds);
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
