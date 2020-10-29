using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Framework.Utilities;
using System;

namespace NeoTestApp.Code
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Gui _gui;
		public static ContentManager MyContent;
		public static GraphicsDevice GraphicsDevice;
		private Camera camera = new Camera(1);


	//	Rectangle rectangle;

		RectangleMap rectangleMap;

		public static MonoGamePlatform CurrentPlatform;

		public Game1(MonoGamePlatform platform)
		{
	
			CurrentPlatform = platform;
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			_graphics.SynchronizeWithVerticalRetrace = false;
			this.IsFixedTimeStep = false;
		//	this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 25); //60);

			rectangleMap = new RectangleMap(this);


			Components.Add(rectangleMap);
			if (platform == MonoGamePlatform.DesktopGL)
			{
				IsMouseVisible = true;
			}
			else if (platform == MonoGamePlatform.Android)
			{
				_graphics.IsFullScreen = true;
				TouchPanel.EnabledGestures = GestureType.HorizontalDrag | GestureType.VerticalDrag | GestureType.Pinch;
			}
		}

		protected override void Initialize()
		{
			GraphicsDevice = base.GraphicsDevice;
			_graphics.PreferredBackBufferWidth = 1920;//Convert.ToInt32(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1.1f);
			_graphics.PreferredBackBufferHeight = 1080; //Convert.ToInt32(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.1f);
			_graphics.ApplyChanges();

			base.Initialize();
			
		//	_gui = new Gui(_graphics, Content);
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnResize;

		//	rectangle = new Rectangle(Vector2.Zero, Rectangle.Techniques.Normal);
		}

		public void OnResize(Object sender, EventArgs e)
		{
			_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

			//_gui.Calculate();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			MyContent = Content;
			_fpsFont = Content.Load<SpriteFont>("patuaone-med");

		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

/*			rectangleMap.View = camera.View;
			rectangleMap.Projection = camera.Projection;*/
		//	_gui.Update(gameTime);

			camera.Update(gameTime);
		}


		private SpriteFont _fpsFont;
		int framecount;
		double timepassed;
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin();
		//	_gui.Draw(_spriteBatch);
			_spriteBatch.End();

			//texture.Draw(gameTime, camera, Vector3.Zero);
			//rectangle.Draw(gameTime, camera, Vector3.Zero);
			base.Draw(gameTime);
			//FPS counter
			framecount++;
			timepassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
			double deltaTime = Math.Round(framecount / timepassed, 1);
			

			if (framecount > 1000)
			{
				framecount = 0;
				timepassed = 0;
			}

			_spriteBatch.Begin();
			_spriteBatch.DrawString(_fpsFont, deltaTime.ToString(), new Vector2(1, 1), Color.Blue);
			_spriteBatch.End();
		}

		/*		public override void Update(GameTime gameTime)
				{
					_mapComponent.Update(gameTime);
					_units.Update(gameTime);
					_camera.Update(gameTime);
					_gui.Update(gameTime);
				}*/

		MouseState _oldMouseState, _newMouseState;
		private void CheckMouse()
		{
			_oldMouseState = _newMouseState;
			_newMouseState = Mouse.GetState();

			//Check if a mouse click occured
			if (_oldMouseState.LeftButton == ButtonState.Pressed && _newMouseState.LeftButton == ButtonState.Released)
			{
				//Handle click
				//	if (_gui.WantsMouse(_newMouseState.Position))
				//	{
				//		_gui.Click(_newMouseState.Position);
				//		return;
				//	}

				/*				if (_units.HasMouseFocus(_newMouseState.Position))
								{
									_units.Click();
									return;
								}

								if (_mapComponent.HasMouseFocus(_newMouseState.Position))
								{
									_mapComponent.Click();
									return;
								}
								else
								{
									_units.ClearSelection();
									_mapComponent.ClearSelection();
									return;
								}*/
			}
			else
			{
				// No click, just hovering..
				/*				if (!_gui.HasMouseFocus(_newMouseState.Position))
									if (!_units.HasMouseFocus(_newMouseState.Position))
										if (!_mapComponent.HasMouseFocus(_newMouseState.Position))
											return;*/
			}

		}

		private void CheckTouch()
		{
			TouchCollection tc = TouchPanel.GetState();
			foreach (TouchLocation tl in tc)
			{
				if (tl.State == TouchLocationState.Pressed)
				{
					// Execute your domain-specific code here
					Point pos = tl.Position.ToPoint();

					//Handle click
					//		if (_gui.WantsMouse(pos))
					//		{
					//			_gui.Click(pos);
					//			return;
					//		}

					/*					if (_units.HasMouseFocus(pos))
										{
											_units.Click();
											return;
										}

										if (_mapComponent.HasMouseFocus(pos))
										{
											_mapComponent.Click();
											return;
										}
										else
										{
											_units.ClearSelection();
											_mapComponent.ClearSelection();
											return;
										}*/
				}
			}
		}
	}
}
