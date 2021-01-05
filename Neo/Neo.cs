using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;
using Neo.Controls;
using System;

namespace Neo
{
	public partial class Neo : Grid
	{
		public Style Style { get; private set; }
		private MonoGamePlatform _currentPlatform;

		private Game _game;
		public float Scale;
		private GuiBatch _guiBatch;
		private Texture2D _texture;
		private SpriteFont _guiFont;

		public Neo(Game game, Style style, MonoGamePlatform platform)
		{
			_game = game;
			_currentPlatform = platform;
			_guiFont = game.Content.Load<SpriteFont>("patuaone-med");
			Style = style;

			_game.Window.ClientSizeChanged += OnResize;		

			if (_currentPlatform == MonoGamePlatform.Android)
				Scale = 2.5f;
			else
				Scale = 1f;
		}

		private void OnResize(object sender, EventArgs e) { ForceRefresh(); }

		public void Init()
		{
			Initialize(this);
			_guiBatch = new GuiBatch(_game.GraphicsDevice, _game.Content.Load<Effect>("effecty"), _game.Content.Load<Effect>("InstancingRectangleShader"), this);
			_texture = _game.Content.Load<Texture2D>("yup");
		}

		internal override void Initialize(Neo neo)
		{
			SetBounds(new Rectangle(0, 0, (int)(_game.GraphicsDevice.Viewport.Width / Scale), (int)(_game.GraphicsDevice.Viewport.Height / Scale)));
		}

		public void ForceRefresh()
		{
			SetBounds(new Rectangle(0, 0, (int)(_game.GraphicsDevice.Viewport.Width / Scale), (int)(_game.GraphicsDevice.Viewport.Height / Scale)));
			_guiBatch.ForceRefresh();
		}

		public void Draw(GameTime gameTime)
		{
			_guiBatch.Begin();

			foreach (Control c in this)
				c.Draw(gameTime, _guiBatch);

			_guiBatch.End();
		}

		public new bool ListensForMouseOrTouchAt(Point mouseOrTouchPosition)
		{
			Point pos = new Point((int)(mouseOrTouchPosition.X / Scale), (int)(mouseOrTouchPosition.Y / Scale));

			foreach (Control c in this)
				if (c.ListensForMouseOrTouchAt(pos))
					return true;

			return false;
		}

		public new bool Click(Point mouseOrTouchPosition)
		{
			Point pos = new Point((int)(mouseOrTouchPosition.X / Scale), (int)(mouseOrTouchPosition.Y / Scale));

			foreach (Control c in this)
				if (c.ListensForMouseOrTouchAt(pos))
				{
					if (c.Click(pos))
						return true;
				}

				return false;
		}

		#region Old code
		/*		public void Calculate(bool force)
				{
					CalculateScale();
					foreach (Control control in _controls)
						if (control.HasChanged || force)
							ComputeControl(control, new Rectangle(0, 0, _screenSize.X, _screenSize.Y), _controls.Count-1, 0);
				}*/

		/*		public void Update(GameTime gameTime)
				{
					if (_currentPlatform == MonoGamePlatform.DesktopGL)
						CheckMouse();
					else if (_currentPlatform == MonoGamePlatform.Android)
						CheckTouch();
				}*/
		/*
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
		*/
		/*		private void CheckTouch()
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
				}*/

		/*		Control moused = null;
				public Control GetControlWithMouse(Point point)
				{
					moused = null;
					foreach (Control control in _controls)
						if (control.Bounds.Contains(point))
							ControlHasMouse(control, point);

					if (moused != null)
						return moused;

					return null;
				}*/

		/*		public void Click(Point point)
				{
					moused.Click();
				}*/

		/*		private void ComputeControl(Control control, Rectangle parentBounds, int numSiblings, int siblingIndex)
				{
					control.Initialize(this);

					for (int i = 0; i < control.ChildCount; i++)
						ComputeControl(control[i], control.Bounds, control.ChildCount-1, i);
				}*/

		/*		private void ControlHasMouse(Control control, Point point)
				{
					if (control.Bounds.Contains(point))
					{
						moused = control;
						foreach (Control child in control)
							ControlHasMouse(child, point);
					}
				}*/

		#endregion
	}
}
