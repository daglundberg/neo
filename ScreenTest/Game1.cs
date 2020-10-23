using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ScreenTest
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Gui _gui;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			this.IsFixedTimeStep = true;//false;
			this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 25); //60);
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = Convert.ToInt32(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1.1f);
			_graphics.PreferredBackBufferHeight = Convert.ToInt32(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.1f);
			_graphics.ApplyChanges();

			base.Initialize();
			_gui = new Gui(Content, new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			_gui.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Indigo);
			_spriteBatch.Begin();
			_gui.Draw(_spriteBatch);
			_spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
