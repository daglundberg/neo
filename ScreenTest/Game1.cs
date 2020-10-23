using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neo.Components;
using System;

namespace ScreenTest
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Gui gui;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = Convert.ToInt32(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1.1f);
			_graphics.PreferredBackBufferHeight = Convert.ToInt32(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.1f);
			_graphics.ApplyChanges();

			base.Initialize();
			gui = new Gui(Content, new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			gui.Update();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Indigo);
			_spriteBatch.Begin();
			gui.Draw(_spriteBatch);
			_spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
