﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Framework.Utilities;
using System;
using Neo;

namespace NeoTestApp.Code
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
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
			_graphics.PreferredBackBufferWidth = 1920;
			_graphics.PreferredBackBufferHeight = 1080;
			_graphics.ApplyChanges();
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnResize;

			base.Initialize();

			_gui = new Gui(this);
			_gui.Init();
		}

		public void OnResize(Object sender, EventArgs e)
		{
			_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

		//	_gui.Calculate();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_fpsFont = Content.Load<SpriteFont>("patuaone-med");
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_gui.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Indigo);

			_gui.Draw(gameTime);

			base.Draw(gameTime);

			DrawFPSCounter(gameTime);
		}

		private SpriteFont _fpsFont;
		int framecount;
		double timepassed;
		private void DrawFPSCounter(GameTime gameTime)
		{
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
	}
}
