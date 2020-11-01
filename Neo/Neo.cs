using Microsoft.Xna.Framework;
using MonoGame.Framework.Utilities;
using Neo.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo
{
	public partial class Neo : Grid
	{
		public Style Style { get; private set; }
		private MonoGamePlatform _currentPlatform;
		private InstancedRectangles _instancedRectangles;
		private Game _game;

		public Neo(Game game, Style style, MonoGamePlatform platform)
		{
			_game = game;
			_currentPlatform = platform;

			Style = style;
			_instancedRectangles = new InstancedRectangles(game);

		}

		public void Init()
		{
			Initialize(this);
		}

		List<Block> blocks;
		internal override void Initialize(Neo neo)
		{

			SetBounds(new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height));

			blocks = new List<Block>();

/*						foreach (Control child in this)
							blocks.Add(child.Block);*/


			GetBlocksRecursively(this);

			_instancedRectangles.SetBlocks(blocks.Skip(1).ToArray());
		}

		private void GetBlocksRecursively(Control control)
		{
			blocks.Add(control.Block);
			foreach (Control c in control)
				GetBlocksRecursively(c);
		}

		public void Draw(GameTime gameTime)
		{
			_instancedRectangles.Draw(gameTime);
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
