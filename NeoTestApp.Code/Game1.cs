using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Framework.Utilities;
using System;

namespace NeoTestApp.Code
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private Gui _gui;

		public static MonoGamePlatform CurrentPlatform;

		public Game1(MonoGamePlatform platform)
		{
			CurrentPlatform = platform;
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			if (CurrentPlatform == MonoGamePlatform.Android)
				_graphics.IsFullScreen = true;
			else
			{
				_graphics.PreferredBackBufferWidth = 800;
				_graphics.PreferredBackBufferHeight = 600;
				Window.AllowUserResizing = true;
				Window.ClientSizeChanged += OnResize;
			}
			_graphics.ApplyChanges();


			base.Initialize();

			_gui = new Gui(this);
		}

		public void OnResize(Object sender, EventArgs e)
		{
			_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
		}

		protected override void LoadContent() { }

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_gui.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(new Color(40,20,50));
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			base.Draw(gameTime);

			_gui.Draw(gameTime);
		}
	}
}
